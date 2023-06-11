namespace Shared.Contracts.Base.Mapper;

public interface IMapper<TLeftObject, TRightObject>
{
    TLeftObject? LooseMap(TRightObject? inObject);

    TRightObject? LooseMap(TLeftObject? inObject);

    TLeftObject Map(TRightObject? inObject);

    TRightObject Map(TLeftObject? inObject);
}