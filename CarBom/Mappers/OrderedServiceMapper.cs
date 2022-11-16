using CarBom.Responses;
using CarBom.Utils;
using DataProvider.DataModels;

namespace CarBom.Mappers
{
    public class OrderedServiceMapper : IOrderedServiceMapper
    {
        public List<OrderedServiceResponse> MapOrderedServices(List<OrderedService> orderedServices)
        {
            List<OrderedServiceResponse> orderedServiceResponses = new List<OrderedServiceResponse>(0);

            foreach (var orderedService in orderedServices)
            {
                OrderedServiceResponse orderedServiceResponse = new OrderedServiceResponse
                {
                    Name = orderedService.Name,
                    Mechanic = orderedService.Mechanic,
                    CreatedDate = DateFormatterUtil.FormatDateToISO8601Pattern(orderedService.CreatedDate),
                    FormattedDate = DateFormatterUtil.FormatDateToCarBomPattern(orderedService.CreatedDate)
                };
                orderedServiceResponses.Add(orderedServiceResponse);
            }
            return orderedServiceResponses;
        }
    }

    public interface IOrderedServiceMapper
    {
        List<OrderedServiceResponse> MapOrderedServices(List<OrderedService> orderedServices);
    }
}
