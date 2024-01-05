using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using YSJU.ClientRegistrationSystem.Dtos.ResponseDtos;
using YSJU.ClientRegistrationSystem.Dtos.TransactionManagementDtos;

namespace YSJU.ClientRegistrationSystem.Interfaces.TransactionManagement
{
    public interface ITransactionManagementAppService: IApplicationService
    {
        Task<ResponseDto<TransactionResponseDto>> CreateSellTransactionAsync(CreateTransactionDto input);
        Task<PagedResultDto<TransactionResponseDto>> GetPagedAndSortedSellTrasactionListAsync(PagedAndSortedTransactionDto input);
    }
}
