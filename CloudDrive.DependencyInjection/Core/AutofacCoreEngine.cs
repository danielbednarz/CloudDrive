using Autofac;
using CloudDrive.Application;
using CloudDrive.Data.Abstraction;
using CloudDrive.Data.Repositories;
using CloudDrive.EntityFramework;

namespace CloudDrive.DependencyResolver
{
    public class AutofacCoreEngine : Module
    {
        public AutofacCoreEngine()
        {

        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainDatabaseContext>().AsSelf().PropertiesAutowired().InstancePerLifetimeScope();

            builder.RegisterType<FileRepository>().As<IFileRepository>().InstancePerLifetimeScope();
            builder.RegisterType<FileRepository>().As<FileRepository>().InstancePerLifetimeScope();

            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<UserRepository>().InstancePerLifetimeScope();

            builder.RegisterType<FileService>().As<IFileService>().InstancePerLifetimeScope();
            builder.RegisterType<FileService>().As<FileService>().InstancePerLifetimeScope();

            builder.RegisterType<TokenService>().As<ITokenService>().InstancePerLifetimeScope();
            builder.RegisterType<TokenService>().As<TokenService>().InstancePerLifetimeScope();

            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<UserService>().InstancePerLifetimeScope();
        }
    }

}
