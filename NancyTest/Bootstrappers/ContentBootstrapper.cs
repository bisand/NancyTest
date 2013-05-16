using Nancy;
using Nancy.Conventions;

namespace NancyTest.Bootstrappers
{
public class ContentBootstrapper : DefaultNancyBootstrapper
{
    protected override void ConfigureConventions(NancyConventions conventions)
    {
        base.ConfigureConventions(conventions);

        conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("css", @"Content/css"));
        conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("js", @"Content/js"));
        conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("images", @"Content/images"));
    }
}}