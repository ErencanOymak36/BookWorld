using AutoMapper;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorld.Application.DI
{
    public class DIModule : NinjectModule
    {
        public override void Load()
        {
            // Service binding
            //Bind<IBookService>().To<BookService>().InSingletonScope();

            //// AutoMapper binding
            //Bind<IMapper>().ToMethod(context =>
            //{
            //    var config = new MapperConfiguration(cfg =>
            //    {
            //        cfg.AddProfile<MappingProfile>();
            //    });
            //    return config.CreateMapper();
            //}).InSingletonScope();
        }
    }
}
