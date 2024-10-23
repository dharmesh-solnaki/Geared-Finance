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
    private readonly IBaseRepo<RateChartOption> _rateChartOptionRepo;
    private readonly IBaseRepo<InterestChart> _interestChartRepo;
    private readonly IBaseRepo<InterestChartFunding> _chartFundingRepo;


    public FunderService(
      IFunderRepo repo,
      IAuthService authService,
      IBaseRepo<Document> docRepo,
      IBaseRepo<FunderProductGuide> productGuideRepo,
      IBaseRepo<FunderProductFunding> fundingRepo,
      IBaseRepo<Note> noteRepo,
      IBaseRepo<RateChartOption> rateChartOptionRepo,
      IBaseRepo<InterestChart> interestChartRepo,
      IBaseRepo<InterestChartFunding> chartFundingRepo) : base(repo)
    {
        _funderRepo = repo;
        _authService = authService;
        _docRepo = docRepo;
        _productGuideRepo = productGuideRepo;
        _fundingRepo = fundingRepo;
        _noteRepo = noteRepo;
        _rateChartOptionRepo = rateChartOptionRepo;
        _interestChartRepo = interestChartRepo;
        _chartFundingRepo = chartFundingRepo;
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
            Includes = new Expression<Func<Funder, object>>[] { x => x.FunderProductGuide, x => x.RateChartOptions.Where(x => !x.IsDeleted) },
            PageNumber = searchModel.PageNumber,
            PageSize = searchModel.PageSize,
            SortBy = searchModel.SortBy,
            SortOrder = searchModel.SortOrder,
        };
        baseSearchEntity.SetSortingExpression();
        IQueryable<Funder> funders = await GetAllAsync(baseSearchEntity);
        return await funders.GetBaseResponseAsync(searchModel.PageNumber, searchModel.PageSize, funder => funder.ToDisplayFunderDTO());
    }

    public async Task<FunderDTO> GetFunder(int id)
    {
        Funder funder = await _funderRepo.GetOneAsync(x => x.Id == id && !x.IsDeleted, null) ?? throw new KeyNotFoundException(Constants.RECORD_NOT_FOUND);
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
        funderGuide.FunderProductFundings = (_funderRepo.GetFundings(funderGuide.Id)).ToList();

        BaseSearchEntity<RateChartOption> search = new()
        {
            Predicate = x => x.FunderId == id && !x.IsDeleted,
            Includes = new Expression<Func<RateChartOption, object>>[] { x => x.InterestChartFundings }
        };
        IQueryable<RateChartOption> rateChartopts = await _rateChartOptionRepo.GetAllAsync(search);

        SelectedFundingDTO[] selectedFundings = Array.Empty<SelectedFundingDTO>();
        if (funderGuide.FunderProductFundings.Any())
        {
            selectedFundings = funderGuide.FunderProductFundings.ToList().ToDto();
        }
        FunderGuideTypeDTO funder = funderGuide.ToDto();
        funder.SelectedFundings = selectedFundings;
        funder.BeingUsedFunding = rateChartopts.SelectMany(x => x.InterestChartFundings.Select(x => x.EquipmentId));
        funder.IsChattelTypeExist = rateChartopts.Any(x => x.TypeOfFinance.Contains("Chattel mortgage"));
        funder.IsRentalTypeExist = rateChartopts.Any(x => x.TypeOfFinance.Contains("Rental"));
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
        await SetFunderFundings(funderProductGuide.Id, selectedFundings);
        return funderProductGuide.Id;
    }

    private async Task SetFunderFundings(int funderGuideId, List<FunderProductFunding> selectedFundings)
    {
        IQueryable<FunderProductFunding> existingFundingList = _funderRepo.GetFundings(funderGuideId);
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
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(logoImage.FileName);
        string fileExtension = Path.GetExtension(logoImage.FileName);
        string imageName = $"{fileNameWithoutExtension}Funder{id}{fileExtension}";
        path = Path.Combine(path, imageName);
        using var stream = new FileStream(path, FileMode.Create);
        await logoImage.CopyToAsync(stream);

        funder.LogoImg = imageName;
        await _funderRepo.UpdateAsync(funder);
    }

    public async Task SaveDocumentAsync(int id, IFormFile doc)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Documents");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(doc.FileName);
        string fileExtension = Path.GetExtension(doc.FileName);
        string fileName = $"{fileNameWithoutExtension}Funder{id}{fileExtension}";
        path = Path.Combine(path, fileName);
        using var stream = new FileStream(path, FileMode.Create);
        await doc.CopyToAsync(stream);

        Document document = new()
        {
            FileName = fileName,
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
        return await docs.GetBaseResponseAsync(searchModel.PageNumber, searchModel.PageSize, doc => doc.ToDto());
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
            IQueryable<FunderProductFunding> fundings = _funderRepo.GetFundings(funderProductGuide.Id);
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
        return await notes.GetBaseResponseAsync(searchModel.PageNumber, searchModel.PageSize, note => note.ToDto());

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

    public async Task<RateChartResponseDTO> GetRateChartsAsync(int funderId)
    {
        BaseSearchEntity<RateChartOption> baseSearchEntity = new()
        {
            Predicate = x => x.FunderId == funderId && !x.IsDeleted,
            Includes = new Expression<Func<RateChartOption, object>>[] { x => x.InterestChartFundings, x => x.InterestCharts },
            ThenIncludes = new Expression<Func<RateChartOption, object>>[]{  x=>((InterestChartFunding)x.InterestChartFundings).Equipment,
            x=>((InterestChartFunding)x.InterestChartFundings).EquipmentCategory, },

        };
        IQueryable<RateChartOption> rateChartOptions = await _rateChartOptionRepo.GetAllAsync(baseSearchEntity);


        BaseSearchEntity<FunderProductGuide> fundingSearch = new()
        {
            Predicate = x => x.FunderId == funderId,
            Includes = new Expression<Func<FunderProductGuide, object>>[] { x => x.FunderProductFundings },
            ThenIncludes = new Expression<Func<FunderProductGuide, object>>[] {
                x => ((FunderProductFunding)x.FunderProductFundings).EquipmentCategory, x => ((FunderProductFunding)x.FunderProductFundings).Equipment, }
        };

        FunderProductGuide funderProductGuide = (await _productGuideRepo.GetAllAsync(fundingSearch)).FirstOrDefault() ?? throw new KeyNotFoundException(Constants.RECORD_NOT_FOUND);


        RateChartResponseDTO rateChartRes = new()
        {
            RateCharts = rateChartOptions.AsEnumerable().Select(x => x.ToDto()),
            AvailableFundings = (funderProductGuide.FunderProductFundings.ToList()).ToDto(),
            TypeOfFinance = funderProductGuide.TypeOfFinance ?? string.Empty
        };

        return rateChartRes;
    }

    public async Task UpsertRateChartOptionAsync(int funderId, List<RateChartOptionDTO> ratechartOptionsDto)
    {

        int userId = _authService.GetUserId();
        List<RateChartOption> addRateChartList = new();
        List<RateChartOption> updateRateChartList = new();


        foreach (var rateChart in ratechartOptionsDto)
        {
            RateChartOption rateChartOption = rateChart.FromDto();
            rateChartOption.FunderId = funderId;
            if (rateChart.Id == 0)
            {
                rateChartOption.CreatedBy = userId;
                rateChartOption.CreatedDate = DateTime.UtcNow;
                rateChart.Id = rateChartOption.Id;
                addRateChartList.Add(rateChartOption);
            }
            else
            {
                rateChartOption.ModifiedBy = userId;
                rateChartOption.ModifiedDate = DateTime.UtcNow;
                updateRateChartList.Add(rateChartOption);
            }
        }

        if (addRateChartList.Any())
        {
            await _rateChartOptionRepo.AddRangeAsync(addRateChartList);
        }
        if (updateRateChartList.Any())
        {
            await _rateChartOptionRepo.UpdateRangeAsync(updateRateChartList);
        }

        foreach (var rateChart in ratechartOptionsDto)
        {
            //RateChartOption rateChartOption = addRateChartList.FirstOrDefault(x => x.Id == rateChart.Id) ?? updateRateChartList.FirstOrDefault(x => x.Id == rateChart.Id);
            RateChartOption rateChartOption = addRateChartList.FirstOrDefault(rc => rc.EquipmentChartName == rateChart.EquipmentChartName) ?? updateRateChartList.FirstOrDefault(rc => rc.EquipmentChartName == rateChart.EquipmentChartName);

            List<InterestChartFunding> chartFundings = rateChart.SelectedFunding.FromDto(rateChartOption.Id);
            await SetChartFundings(rateChartOption.Id, chartFundings);

            if (rateChart.InterestChartChattelMortgage != null)
            {
                await UpsertInterestCharts(rateChart.InterestChartChattelMortgage, userId, rateChartOption.Id, 1);

            }
            if (rateChart.InterestChartRental != null)
            {
                await UpsertInterestCharts(rateChart.InterestChartRental, userId, rateChartOption.Id, 2);
            }
        }
    }


    private async Task UpsertInterestCharts(IEnumerable<InterestChartDTO> interestChartDTOs, int userId, int rateChartOptionId, int type)
    {

        List<InterestChart> addList = new();
        List<InterestChart> updateList = new();
        List<InterestChart> removeList = new();

        BaseSearchEntity<InterestChart> interestSearch = new()
        {
            Predicate = x => x.RateChartId == rateChartOptionId && !x.IsDeleted && x.TypeOfFinance == (type == 1 ? "Chattel mortgage" : "Rental"),
        };
        IQueryable<InterestChart> oldCharts = await _interestChartRepo.GetAllAsync(interestSearch);
        var oldChartDict = oldCharts.ToDictionary(chart => chart.Id);
        foreach (var chart in interestChartDTOs)
        {
            InterestChart interestChart = chart.FromDto();
            interestChart.RateChartId = rateChartOptionId;

            if (interestChart.Id == 0)
            {
                interestChart.CreatedBy = userId;
                interestChart.CreatedDate = DateTime.UtcNow;
                addList.Add(interestChart);
            }
            else if (oldChartDict.ContainsKey(interestChart.Id))
            {
                interestChart.ModifiedBy = userId;
                interestChart.ModifiedDate = DateTime.UtcNow;
                updateList.Add(interestChart);
                oldChartDict.Remove(interestChart.Id);
            }
        }
        foreach (var oldChart in oldChartDict.Values)
        {
            oldChart.IsDeleted = true;
            oldChart.ModifiedBy = userId;
            oldChart.ModifiedDate = DateTime.UtcNow;
            removeList.Add(oldChart);
        }
        if (addList.Count > 0)
            await _interestChartRepo.AddRangeAsync(addList);

        if (updateList.Count > 0)
            await _interestChartRepo.UpdateRangeAsync(updateList);

        if (removeList.Count > 0)
            await _interestChartRepo.UpdateRangeAsync(removeList);
    }

    private async Task SetChartFundings(int chartId, List<InterestChartFunding> selectedFundings)
    {
        BaseSearchEntity<InterestChartFunding> searchQuery = new()
        {
            Predicate = x => x.ChartEquipmentId == chartId,
            Includes = new Expression<Func<InterestChartFunding, object>>[] { x => x.Equipment, x => x.EquipmentCategory }
        };
        IQueryable<InterestChartFunding> existingFundingList = await _chartFundingRepo.GetAllAsync(searchQuery);
        if (!existingFundingList.Any())
        {
            await _chartFundingRepo.AddRangeAsync(selectedFundings);
        }
        else
        {
            var fundingsToAdd = selectedFundings
                       .Where(selected => !existingFundingList
                           .Any(existing =>
                               existing.EquipmentId == selected.EquipmentId &&
                               existing.EquipmentCategoryId == selected.EquipmentCategoryId &&
                               existing.ChartEquipmentId == selected.ChartEquipmentId))
                       .ToList();

            var fundingsToRemove = existingFundingList
                    .AsEnumerable()
                    .Where(existing => !selectedFundings
                        .Any(selected =>
                            selected.EquipmentId == existing.EquipmentId &&
                            selected.EquipmentCategoryId == existing.EquipmentCategoryId &&
                            selected.ChartEquipmentId == existing.ChartEquipmentId))
                    .ToList();

            await _chartFundingRepo.AddRangeAsync(fundingsToAdd);
            await _chartFundingRepo.DeleteRange(fundingsToRemove);
        }
    }

    public async Task DeleteRateChartAsync(int id)
    {
        int userId = _authService.GetUserId();
        BaseSearchEntity<RateChartOption> search = new()
        {
            Predicate = x => x.Id == id && !x.IsDeleted,
            Includes = new Expression<Func<RateChartOption, object>>[] { x => x.InterestCharts, x => x.InterestChartFundings }
        };
        RateChartOption rateChart = await (await _rateChartOptionRepo.GetAllAsync(search)).FirstOrDefaultAsync() ?? throw new KeyNotFoundException(Constants.RECORD_NOT_FOUND);

        if (rateChart.InterestChartFundings.Any())
        {
            await _chartFundingRepo.DeleteRange(rateChart.InterestChartFundings);
        }

        var activeInterestCharts = rateChart.InterestCharts.Where(x => x.RateChartId == rateChart.Id && !x.IsDeleted).ToList();
        if (rateChart.InterestCharts != null)
        {
            foreach (var item in rateChart.InterestCharts)
            {
                item.ModifiedBy = userId;
                item.ModifiedDate = DateTime.UtcNow;
                item.IsDeleted = true;
            }
        }
        rateChart.ModifiedDate = DateTime.UtcNow;
        rateChart.IsDeleted = true;
        rateChart.ModifiedBy = userId;

        await _rateChartOptionRepo.UpdateAsync(rateChart);
    }
}
