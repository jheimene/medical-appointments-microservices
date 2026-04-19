using DispatchService.Application.Commmon.Interfaces;
using MediatR;

namespace DispatchService.Application.Customers.Commands.AddCustomerAddress
{
    public sealed class AddCustomerAddressCommnadHandler : IRequestHandler<AddCustomerAddressCommand, Guid>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddCustomerAddressCommnadHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            this._customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(AddCustomerAddressCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(new CustomerId(request.CustomerId));

            if (customer == null) throw new ArgumentNullException("El cliente no existe"); 

            var customerAddress = customer.AddAddress(
                new CustomerId(request.CustomerId), 
                request.Label, 
                request.Street, 
                request.District, 
                request.Province, 
                request.Departament, 
                "system", 
                null, 
                request.IsDefault);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return customerAddress.Id.Value;
        }
    }
}
