using AutoMapper;
using Shared.Base.Exception;
using Shared.Contracts.Base.Mapper;

namespace Shared.Base.Mapper;

public class EntityMapper<TLeftEntity, TRightEntity> : IMapper<TLeftEntity, TRightEntity>
{
    private IMapper _mapper;


    public EntityMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public TLeftEntity? LooseMap(TRightEntity? inObject)
    {
        return _mapper.Map<TLeftEntity>(inObject);
    }

    public TRightEntity? LooseMap(TLeftEntity? inObject)
    {
        return _mapper.Map<TRightEntity>(inObject);
    }

    public TLeftEntity Map(TRightEntity? inObject)
    {
        return StrictMap<TRightEntity, TLeftEntity>(inObject);
    }

    public TRightEntity Map(TLeftEntity? inObject)
    {
        return StrictMap<TLeftEntity, TRightEntity>(inObject);
    }

    private TOut StrictMap<TIn, TOut>(TIn? inObject)
    {
        var mapped = _mapper.Map<TOut>(inObject);
        if (mapped == null)
        {
            throw new MappingException();
        }

        return mapped;
    }
}