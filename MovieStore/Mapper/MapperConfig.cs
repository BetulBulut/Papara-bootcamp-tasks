using AutoMapper;
using MovieStore.Models;
using MovieStore.Schema;


namespace MovieStore.Mapper;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<CustomerRequest, Customer>();
        CreateMap<Customer, CustomerResponse>();

        CreateMap<OrderRequest, Order>();
        CreateMap<Order, OrderResponse>();

        CreateMap<MovieRequest, Movie>();
        CreateMap<Movie, MovieResponse>();
        
        CreateMap<DirectorRequest, Director>();
        CreateMap<Director, DirectorResponse>();

        CreateMap<ActorRequest, Actor>();
        CreateMap<Actor, ActorResponse>();

       
    }
}