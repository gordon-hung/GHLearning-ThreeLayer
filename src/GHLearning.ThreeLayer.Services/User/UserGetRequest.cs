using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace GHLearning.ThreeLayer.Services.User;

public record UserGetRequest(
	string Account) : IRequest<UserGetResponse?>;
