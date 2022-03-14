using Autofac;
using CloudDrive.Data.Repositories;
using CloudDrive.Data.Interfaces;
using CloudDrive.EntityFramework;
using CloudDrive.Application;

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

            builder.RegisterType<FileService>().As<IFileService>().InstancePerLifetimeScope();
            builder.RegisterType<FileService>().As<FileService>().InstancePerLifetimeScope();
        }
    }

}
