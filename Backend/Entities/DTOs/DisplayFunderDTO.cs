using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs;
public class DisplayFunderDTO:BaseDTO
{
    public string Funder { get; set; } = null!;
    public string LegalName { get; set; } = null!;
    public string FinanceType { get; set; } = null!;
    public string BdmName { get; set; } = null!;
    public string BdmEmail { get; set; } = null!;
    public string BdmPhone { get; set; } = null!;
    public bool Status { get; set; }

}
