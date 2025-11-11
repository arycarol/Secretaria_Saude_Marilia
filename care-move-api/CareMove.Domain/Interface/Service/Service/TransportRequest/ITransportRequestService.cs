using CareMove.Argument.Argument;
using CareMove.Argument.Base;

namespace CareMove.Domain.Interface.Service.Service;

public interface ITransportRequestService : IBaseService<InputCreateTransportRequest, InputUpdateTransportRequest, InputGenericDelete, OutputTransportRequest> { }