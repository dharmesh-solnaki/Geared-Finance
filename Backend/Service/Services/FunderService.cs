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
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using Utilities;


namespace Service.Services;

public class FunderService : BaseService<Funder>, IFunderService
{
    private readonly IFunderRepo _funderRepo;
    private readonly IAuthService _authService;
    private readonly IBaseRepo<Document> _docRepo;
    private readonly IBaseRepo<FunderProductGuide> _productGuideRepo;
    private readonly IBaseRepo<FunderProductFunding> _fundingRepo;


    public FunderService(IFunderRepo repo, IAuthService authService, IBaseRepo<Document> docRepo , IBaseRepo<FunderProductGuide> productGuideRepo, IBaseRepo<FunderProductFunding> fundingRepo) : base(repo)
    {
        _funderRepo = repo;
        _authService = authService;
        _docRepo = docRepo;
        _productGuideRepo = productGuideRepo;
        _fundingRepo = fundingRepo;
    }

    public async Task<BaseResponseDTO<DisplayFunderDTO>> GetAllFunders(BaseModelSearchEntity searchModel)
    {

        PredicateModel model = new()
        {
            Id = searchModel.id,
            Criteria = new Dictionary<string, object>
            {
            {Constants.ISDELETED,false }
            },
        };

        Expression<Func<Funder, bool>> predicate = PredicateBuilder.BuildPredicate<Funder>(model);
        BaseSearchEntity<Funder> baseSearchEntity = new()
        {
            predicate = predicate,
            includes = new Expression<Func<Funder, object>>[] { x => x.FunderProductGuide },
            pageNumber = searchModel.pageNumber,
            pageSize = searchModel.pageSize,
            sortBy = searchModel.sortBy,
            sortOrder = searchModel.sortOrder,
        };
        baseSearchEntity.SetSortingExpression();
        IQueryable<Funder> funders = await GetAllAsync(baseSearchEntity);
        BaseResponseDTO<DisplayFunderDTO> fudnerResponse = new() { TotalRecords =await funders.CountAsync() };
        List<Funder> userPageList = GetPaginatedList(searchModel.pageNumber, searchModel.pageSize, funders).ToList();
        fudnerResponse.ResponseData = userPageList.Select(funder => funder.ToDisplayFunderDTO()).ToList();
        return fudnerResponse;
    }

    public async Task<FunderDTO> GetFunder(int id)
    {
        Funder funder = await _funderRepo.GetOneAsync(x=>x.Id==id && x.IsDeleted==false,null)??throw new KeyNotFoundException(Constants.RECORD_NOT_FOUND);
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
        //Expression<Func<FunderProductGuide, bool>> predicate = ; 

        //Expression<Func<FunderProductGuide, object>>[] includes = new Expression<Func<FunderProductGuide, object>>[] { x => x.FunderProductFundings , x=>x.FunderProductFundings.Select(f=>f.Equipment) , x=>x.FunderProductFundings.Select(f=>f.EquipmentCategory)};
        FunderProductGuide funderGuide = await _funderRepo.GetByOtherAsync<FunderProductGuide>(x => x.FunderId == id, null);

        if (funderGuide == null)
        {
            return null;
        }
        funderGuide.FunderProductFundings = (await _funderRepo.GetFundingsAsync(funderGuide.Id)).ToList();
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
        if (!funderDTO.id.HasValue || funderDTO.id == 0)
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
        if (!funderGuideTypeDTO.id.HasValue)
        {
            //await _funderRepo.AddFunderGuideAsync(funderProductGuide);
            await _productGuideRepo.AddAsync(funderProductGuide);
        }
        else
        {
            //await _funderRepo.UpdateFunderGuideAsync(funderProductGuide);
            await _productGuideRepo.UpdateAsync(funderProductGuide);
        }


        IQueryable<FunderProductFunding> existingFundingList = await _funderRepo.GetFundingsAsync(funderProductGuide.Id);
        List<FunderProductFunding> selectedFundings = funderGuideTypeDTO.SelectedFundings.FromDto(funderProductGuide.Id);

        if (!existingFundingList.Any())
        {
            //await _funderRepo.AddRangeFunderGuideFundingAsync(selectedFundings);
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



            //await _funderRepo.AddRangeFunderGuideFundingAsync(fundingsToAdd);
            await _fundingRepo.AddRangeAsync (fundingsToAdd);
            await _fundingRepo.DeleteRange(fundingsToRemove);
            //await _funderRepo.RemoveRangeSelectedFundingAsync(fundingsToRemove);
        }
        return funderProductGuide.Id;
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
            predicate = x => x.FunderId == id && x.IsDeleted == false,
            pageNumber = searchModel.pageNumber,
            pageSize = searchModel.pageSize,
            sortBy = searchModel.sortBy,
            sortOrder = searchModel.sortOrder,
        };
        baseSearchEntity.SetSortingExpression();
        IQueryable<Document> docs = await _docRepo.GetAllAsync(baseSearchEntity);
        BaseResponseDTO<DocumentDTO> fudnerResponse = new() { TotalRecords = await docs.CountAsync() };
        List<Document> docList = docs.Skip((searchModel.pageNumber - 1) * searchModel.pageSize).Take(searchModel.pageSize).ToList();
        fudnerResponse.ResponseData = docList.Select(doc => doc.ToDto()).ToList();
        return fudnerResponse;
    }

    public async Task<byte[]> GetPdfDocumentAsync(string docName)
    {
        string docPath = Path.Combine( "Uploads", "Documents", docName);
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
        var filePath = Path.Combine("Uploads","Documents",doc.FileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        doc.IsDeleted = true;
        await _docRepo.UpdateAsync(doc);
    }

    public async Task DeleteFunderAsync(int id)
    {
        Funder funder =await GetByIdAsync(id) ?? throw new KeyNotFoundException(Constants.RECORD_NOT_FOUND);
        BaseSearchEntity<Document> search = new()
        {
            predicate = (x) => x.FunderId == id && x.IsDeleted == false

        };
        IEnumerable<Document> documents = (await _docRepo.GetAllAsync(search)).ToList();
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


          FunderProductGuide funderProductGuide = await _productGuideRepo.GetOneAsync((x) => x.FunderId == id,null);
        if (funderProductGuide != null)
        {

            IQueryable<FunderProductFunding> fundings = await _funderRepo.GetFundingsAsync(funderProductGuide.Id);
            await _fundingRepo.DeleteRange(fundings);

            funderProductGuide.IsDeleted = true;
            await _productGuideRepo.UpdateAsync(funderProductGuide);
        }

        string imgPath = Path.Combine("Uploads", "LogoImages" , funder.LogoImg??"");
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        funder.LogoImg=string.Empty;
        funder.IsDeleted = true;
        await _funderRepo.UpdateAsync(funder);

    }
}
