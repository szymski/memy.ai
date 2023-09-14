namespace WebApi.Dto; 

public interface IBaseEntityDto {
    int Id { get; set; }
    DateTime CreatedAt { get; set; }
}