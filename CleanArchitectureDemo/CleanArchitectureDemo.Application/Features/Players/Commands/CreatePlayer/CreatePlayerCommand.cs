using CleanArchitectureDemo.Application.Interfaces.Repositories;
using MediatR;
using AutoMapper;
using CleanArchitectureDemo.Shared;
using CleanArchitectureDemo.Domain.Entities;
using CleanArchitectureDemo.Application.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDemo.Application.Features.Players.Commands.CreatePlayer
{
    public record CreatePlayerCommand : IRequest<Result<int>>, IMapFrom<Player>
    {
        public string Name { get; set; }
        public int ShirtNo { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime Birthdate { get; set; }
    }
    internal class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreatePlayerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreatePlayerCommand command, CancellationToken cancellationToken)
        {
            var player = new Player()
            {
                Name = command.Name,
                ShirtNo = command.ShirtNo,
                PhotoUrl = command.PhotoUrl,
                Birthdate = command.Birthdate
            };

            await _unitOfWork.Repository<Player>().AddAsync(player);
            player.AddDomainEvent(new PlayerCreatedEvent(player));
            await _unitOfWork.Save(cancellationToken);
            return await Result<int>.SuccessAsync(player.Id, "Player Created.");

        }
    }
}
