using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IFunderService : IBaseService<Funder>
    {
        Task<BaseRepsonseDTO<DisplayFunderDTO>> GetAllFunders(BaseModelSearchEntity searchModel);
        Task<FunderDTO> GetFunder(int id);
        Task<FunderGuideTypeDTO> GetFunderType(int id);
        Task<int> UpsertFunder(FunderDTO funderDTO);
        Task<int> UpsertFunderGuide(FunderGuideTypeDTO funderGuideTypeDTO);
    }
}
