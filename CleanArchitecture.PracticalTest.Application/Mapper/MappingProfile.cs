using AutoMapper;


namespace CleanArchitecture.PracticalTest.Application.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        EnumMappingProfile();

        CommandToDomainMappingProfile();

        DomainToViewModelMappingProfile();
    }

    private void EnumMappingProfile()
    {
        // Mapeo de enumeraciones a DTOs
        // CreateMap<Status, StatusDto>().ReverseMap();        
    }

    private void CommandToDomainMappingProfile()
    {
        // Mapeo de comandos a entidades del dominio
        // CreateMap<CreateEntityCommand, DomainEntity>();

    }

    private void DomainToViewModelMappingProfile()
    {
        // Mapeo de entidades del dominio a ViewModels
        // CreateMap<DomainEntity, DomainViewModel>();
    }
}
