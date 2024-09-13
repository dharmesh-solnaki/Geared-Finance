using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;
using Microsoft.AspNetCore.Http;

namespace Service.Interface
{
    public interface IFunderService : IBaseService<Funder>
    {
        Task DeleteDocAsync(int id);
        Task DeleteFunderAsync(int id);
        Task<BaseResponseDTO<DisplayFunderDTO>> GetAllFunders(BaseModelSearchEntity searchModel);
        Task<BaseResponseDTO<DocumentDTO>> GetDcoumentsAsync(int id, BaseModelSearchEntity searchModel);
        Task<FunderDTO> GetFunder(int id);
        Task<FunderGuideTypeDTO> GetFunderType(int id);
        Task<byte[]> GetPdfDocumentAsync(string docName);
        Task SaveDocumentAsync(int id, IFormFile document);
        Task SaveLogoImageAsync(int id, IFormFile logoImage);
        Task<int> UpsertFunder(FunderDTO funderDTO);
        Task<int> UpsertFunderGuide(FunderGuideTypeDTO funderGuideTypeDTO);
    }
}
