﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.HttpModels.Responses
{
    public record class ErrorResponse(string Message, HttpStatusCode? StatusCode = null);
}
