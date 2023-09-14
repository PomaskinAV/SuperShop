using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.HttpModels.Responses
{
    public record LoginByPasswordResponse(Guid Id, string Name, Guid? ConfirmationCodeId);
}
