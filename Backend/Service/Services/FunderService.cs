using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;
using Geared_Finance_API;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Service.Extensions;
using Service.Implementation;
using Service.Interface;
using System.Linq.Expressions;
using Utilities;


namespace Service.Services;

public class FunderService : BaseService<Funder>, IFunderService
{
    private readonly IFunderRepo _funderRepo;
    private readonly IAuthService _authService;
    private readonly IBaseRepo<Document> _docRepo;
    private readonly IBaseRepo<FunderProductGuide> _productGuideRepo;
    private readonly IBaseRepo<FunderProductFunding> _fundingRepo;
    private readonly IBaseRepo<Note> _noteRepo;


    public FunderService(
      IFunderRepo repo,
      IAuthService authService,
      IBaseRepo<Document> docRepo,
      IBaseRepo<FunderProductGuide> productGuideRepo,
      IBaseRepo<FunderProductFunding> fundingRepo,
      IBaseRepo<Note> noteRepo) : base(repo)
    {
        _funderRepo = repo;
        _authService = authService;
        _docRepo = docRepo;
        _productGuideRepo = productGuideRepo;
        _fundingRepo = fundingRepo;
        _noteRepo = noteRepo;
    }

    public async Task<BaseResponseDTO<DisplayFunderDTO>> GetAllFunders(BaseModelSearchEntity searchModel)
    {

        PredicateModel model = new()
        {
            Id = searchModel.Id,
            Criteria = new Dictionary<string, object>
            {
            {Constants.ISDELETED,false }
            },
        };

        Expression<Func<Funder, bool>> predicate = PredicateBuilder.BuildPredicate<Funder>(model);
        BaseSearchEntity<Funder> baseSearchEntity = new()
        {
            Predicate = predicate,
            Includes = new Expression<Func<Funder, object>>[] { x => x.FunderProductGuide },
            PageNumber = searchModel.PageNumber,
            PageSize = searchModel.PageSize,
            SortBy = searchModel.SortBy,
            SortOrder = searchModel.SortOrder,
        };
        baseSearchEntity.SetSortingExpression();
        IQueryable<Funder> funders = await GetAllAsync(baseSearchEntity);
        return await funders.GetBaseResponseAsync(searchModel.PageNumber,searchModel.PageSize,funder=>funder.ToDisplayFunderDTO());
    }

    public async Task<FunderDTO> GetFunder(int id)
    {
        Funder funder = await _funderRepo.GetOneAsync(x => x.Id == id && x.IsDeleted == false, null) ?? throw new KeyNotFoundException(Constants.RECORD_NOT_FOUND);
        string imgType = string.Empty;
        if (!string.IsNullOrWhiteSpace(funder.LogoImg))
        {
            string imgPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "LogoImages", funder.LogoImg);
            if (File.Exists(imgPath))
            {
                byte[] imgBytes = await File.ReadAllBytesAsync(imgPath);
                funder.LogoImg = Convert.ToBase64String(imgBytes);

                imgType = ExtensionMethods.GetMimeTypeFromExtension(Path.GetExtension(imgPath));
            }
        }
        FunderDTO funderDTO = funder.ToDto();
        funderDTO.ImgType = imgType;
        return funderDTO;
    }

    public async Task<FunderGuideTypeDTO> GetFunderType(int id)
    { 
        FunderProductGuide funderGuide = await _funderRepo.GetByOtherAsync<FunderProductGuide>(x => x.FunderId == id, null) ?? throw new KeyNotFoundException(Constants.RECORD_NOT_FOUND);
        funderGuide.FunderProductFundings = ( _funderRepo.GetFundings(funderGuide.Id)).ToList();
        SelectedFundingDTO[] selectedFundings = Array.Empty<SelectedFundingDTO>();
        if (funderGuide.FunderProductFundings.Any())
        {
            selectedFundings = funderGuide.FunderProductFundings.ToList().ToDto();
        }
        FunderGuideTypeDTO funder = funderGuide.ToDto();
        funder.SelectedFundings = selectedFundings;
        return funder;
    }

    public async Task<int> UpsertFunder(FunderDTO funderDTO)
    {
        Funder funder = funderDTO.FromDto();
        if (!funderDTO.Id.HasValue || funderDTO.Id == 0)
        {
            funder.CreatedBy = _authService.GetUserId();
            await _funderRepo.AddAsync(funder);
        }
        else
        {
            funder.ModifiedDate = DateTime.Now;
            funder.ModifiedBy = _authService.GetUserId();
            await _funderRepo.UpdateAsync(funder);
        }
        return funder.Id;
    }

    public async Task<int> UpsertFunderGuide(FunderGuideTypeDTO funderGuideTypeDTO)
    {
        FunderProductGuide funderProductGuide = funderGuideTypeDTO.FromDto();
        if (!funderGuideTypeDTO.Id.HasValue)
        {
            await _productGuideRepo.AddAsync(funderProductGuide);
        }
        else
        {
            await _productGuideRepo.UpdateAsync(funderProductGuide);
        }       
        List<FunderProductFunding> selectedFundings = funderGuideTypeDTO.SelectedFundings.FromDto(funderProductGuide.Id);
        await SetFunderFundings(funderProductGuide.Id,selectedFundings);

        return funderProductGuide.Id;
    }

    private async Task SetFunderFundings(int funderGuideId, List<FunderProductFunding> selectedFundings)
    {
        IQueryable<FunderProductFunding> existingFundingList =  _funderRepo.GetFundings(funderGuideId);
        if (!existingFundingList.Any())
        {
            await _fundingRepo.AddRangeAsync(selectedFundings);
        }
        else
        {
            var fundingsToAdd = selectedFundings
                       .Where(selected => !existingFundingList
                           .Any(existing =>
                               existing.EquipmentId == selected.EquipmentId &&
                               existing.EquipmentCategoryId == selected.EquipmentCategoryId &&
                               existing.FundingProductGuideId == selected.FundingProductGuideId))
                       .ToList();

            var fundingsToRemove = existingFundingList
                    .AsEnumerable()
                    .Where(existing => !selectedFundings
                        .Any(selected =>
                            selected.EquipmentId == existing.EquipmentId &&
                            selected.EquipmentCategoryId == existing.EquipmentCategoryId &&
                            selected.FundingProductGuideId == existing.FundingProductGuideId))
                    .ToList();

            await _fundingRepo.AddRangeAsync(fundingsToAdd);
            await _fundingRepo.DeleteRange(fundingsToRemove);
        }
    }


    public async Task SaveLogoImageAsync(int id, IFormFile logoImage)
    {
        Funder funder = await GetByIdAsync(id) ?? throw new KeyNotFoundException(Constants.BAD_REQUEST);
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "LogoImages");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        if (funder.LogoImg != null)
        {
            var existingLogoPath = Path.Combine(path, funder.LogoImg);
            if (File.Exists(existingLogoPath))
            {
                File.Delete(existingLogoPath);
            }
        }
        path = Path.Combine(path, logoImage.FileName);
        using var stream = new FileStream(path, FileMode.Create);
        await logoImage.CopyToAsync(stream);

        funder.LogoImg = logoImage.FileName;
        await _funderRepo.UpdateAsync(funder);
    }

    public async Task SaveDocumentAsync(int id, IFormFile doc)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Documents");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        path = Path.Combine(path, doc.FileName);
        using var stream = new FileStream(path, FileMode.Create);
        await doc.CopyToAsync(stream);

        Document document = new()
        {
            FileName = doc.FileName,
            CreatedDate = DateTime.Now,
            IsDeleted = false,
            FunderId = id,
            CreatedBy = _authService.GetUserId(),
        };
        await _docRepo.AddAsync(document);
    }

    public async Task<BaseResponseDTO<DocumentDTO>> GetDcoumentsAsync(int id, BaseModelSearchEntity searchModel)
    {
        BaseSearchEntity<Document> baseSearchEntity = new()
        {
            Predicate = x => x.FunderId == id && x.IsDeleted == false,
            PageNumber = searchModel.PageNumber,
            PageSize = searchModel.PageSize,
            SortBy = searchModel.SortBy,
            SortOrder = searchModel.SortOrder,
        };
        baseSearchEntity.SetSortingExpression();
        IQueryable<Document> docs = await _docRepo.GetAllAsync(baseSearchEntity);
        return  await docs.GetBaseResponseAsync(searchModel.PageNumber, searchModel.PageSize, doc => doc.ToDto());
    }

    public async Task<byte[]> GetPdfDocumentAsync(string docName)
    {
        string docPath = Path.Combine("Uploads", "Documents", docName);
        byte[] docBytes = Array.Empty<byte>();
        if (File.Exists(docPath))
        {
            docBytes = await File.ReadAllBytesAsync(docPath);

        }
        return docBytes;
    }

    public async Task DeleteDocAsync(int id)
    {
        Document doc = await _docRepo.GetByIdAsync(id) ?? throw new KeyNotFoundException();
        var filePath = Path.Combine("Uploads", "Documents", doc.FileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }        
        doc.IsDeleted = true;        
        await _docRepo.UpdateAsync(doc);
    }

    public async Task DeleteFunderAsync(int id)
    {
        Funder funder = await GetByIdAsync(id) ?? throw new KeyNotFoundException(Constants.RECORD_NOT_FOUND);
        BaseSearchEntity<Document> search = new()
        {
            Predicate = (x) => x.FunderId == id && !x.IsDeleted 

        };
        IEnumerable<Document> documents = (await _docRepo.GetAllAsync(search)).ToList();
        if (documents.Any())
        {
            string path = Path.Combine("Uploads", "Documents");
            foreach (var item in documents)
            {
                path = Path.Combine(path, item.FileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                item.IsDeleted = true;
            }
            await _docRepo.UpdateRangeAsync(documents);
        }   


        FunderProductGuide funderProductGuide = await _productGuideRepo.GetOneAsync((x) => x.FunderId == id, null);
        if (funderProductGuide != null)
        {

            IQueryable<FunderProductFunding> fundings =  _funderRepo.GetFundings(funderProductGuide.Id);
            await _fundingRepo.DeleteRange(fundings);

            funderProductGuide.IsDeleted = true;            
            await _productGuideRepo.UpdateAsync(funderProductGuide);
        }

        string imgPath = Path.Combine("Uploads", "LogoImages", funder.LogoImg ?? "");
        if (File.Exists(imgPath))
        {
            File.Delete(imgPath);
        }
        funder.LogoImg = string.Empty;
        funder.IsDeleted = true;
        funder.ModifiedBy = _authService.GetUserId();
        funder.ModifiedDate = DateTime.Now;
        await _funderRepo.UpdateAsync(funder);

    }

    public async Task<IEnumerable<IdNameDTO>> GetFunderSearchList(string name)
    {
        BaseSearchEntity<Funder> funderSearch = new()
        {
            Predicate = x => !x.IsDeleted
        };
        if (name != null)
        {
            name = name.ToLower();
            funderSearch.Predicate = x => (x.Name.ToLower().Contains(name) || x.Bdmname.ToLower().Contains(name) || x.Bdmsurname.ToLower().Contains(name) || x.EntityName.ToLower().Contains(name) || x.Abn.Contains(name)) && !x.IsDeleted;
        }

        IEnumerable<Funder> funders = (await GetAllAsync(funderSearch)).ToList();
        return funders.Select(funder => funder.ToIdNameList());
    }

    public async Task<BaseResponseDTO<NoteDTO>> GetNoteListAsync(int funderId, BaseModelSearchEntity searchModel)
    {
        BaseSearchEntity<Note> baseSearchEntity = new()
        {
            Predicate = x => x.FunderId == funderId && !x.IsDeleted,
            Includes = new Expression<Func<Note, object>>[] { x => x.CreatedByNavigation },
            PageNumber = searchModel.PageNumber,
            PageSize = searchModel.PageSize,
            SortBy = searchModel.SortBy,
            SortOrder = searchModel.SortOrder,
        };
        baseSearchEntity.SetSortingExpression();
        IQueryable<Note> notes = await _noteRepo.GetAllAsync(baseSearchEntity);
        return await notes.GetBaseResponseAsync(searchModel.PageNumber,searchModel.PageSize, note=>note.ToDto());

    }

    public async Task<int> UpsertNoteAsync(NoteDTO noteDto, int funderId)
    {
        Note note = noteDto.FromDto();
        note.FunderId = funderId;
        if (noteDto.Id == 0)
        {
            note.CreatedBy = _authService.GetUserId();
            await _noteRepo.AddAsync(note);
        }
        else
        {
            note.ModifiedDate = DateTime.Now;
            note.ModifiedBy = _authService.GetUserId();
            await _noteRepo.UpdateAsync(note);
        }
        return note.Id;
    }

    public async Task DeleteNoteAsync(int id)
    {
        Note note = await _noteRepo.GetByIdAsync(id) ?? throw new KeyNotFoundException(Constants.RECORD_NOT_FOUND);
        note.ModifiedBy = _authService.GetUserId();
        note.ModifiedDate = DateTime.Now;
        note.IsDeleted = true;
        await _noteRepo.UpdateAsync(note);
    }
}
