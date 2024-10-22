using System;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates;

public class RandomizeAuthServerPassPhraseStep : ProjectBuildPipelineStep
{
    private const string DefaultPassword = "00000000-0000-0000-0000-000000000000";
    private const string KestrelCertificatesDefaultPassword = "Kestrel__Certificates__Default__Password=00000000-0000-0000-0000-000000000000";
    private const string LocalhostPfx = "localhost.pfx -p 00000000-0000-0000-0000-000000000000";
    private const string DotnetDevCerts = "openiddict.pfx -p 00000000-0000-0000-0000-000000000000";
    private const string ProductionEncryptionAndSigningCertificate = "AddProductionEncryptionAndSigningCertificate(\"openiddict.pfx\", \"00000000-0000-0000-0000-000000000000\");";
    private const string ReadmeCallout = "`00000000-0000-0000-0000-000000000000`";

    private readonly static string RandomPassword = Guid.NewGuid().ToString("D");
    public readonly static string RandomOpenIddictPassword = Guid.NewGuid().ToString("D");

    public override void Execute(ProjectBuildContext context)
    {
        var files = context.Files
            .Where(x => !x.IsDirectory)
            .Where(x => x.Name.EndsWith(".cs") ||
                        x.Name.EndsWith(".json") ||
                        x.Name.EndsWith(".yml") ||
                        x.Name.EndsWith(".yaml") ||
                        x.Name.EndsWith(".md") ||
                        x.Name.EndsWith(".ps1") ||
                        x.Name.EndsWith(".sh") ||
                        x.Name.Contains("Dockerfile"))
            .Where(x => x.Content.IndexOf(DefaultPassword, StringComparison.InvariantCultureIgnoreCase) >= 0)
            .ToList();

        string module = null;
        foreach (var file in files)
        {
            file.NormalizeLineEndings();

            var lines = file.GetLines();
            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(KestrelCertificatesDefaultPassword))
                {
                    lines[i] = lines[i].Replace(KestrelCertificatesDefaultPassword,
                        KestrelCertificatesDefaultPassword.Replace(DefaultPassword,
                            RandomPassword));
                }

                if (lines[i].Contains(LocalhostPfx))
                {
                    lines[i] = lines[i].Replace(LocalhostPfx,
                        LocalhostPfx.Replace(DefaultPassword,
                            RandomPassword));
                }

                if (lines[i].Contains(DotnetDevCerts))
                {
                    lines[i] = lines[i].Replace(DotnetDevCerts,
                        DotnetDevCerts.Replace(DefaultPassword,
                            RandomOpenIddictPassword));
                }

                if (lines[i].Contains(ReadmeCallout))
                {
                    lines[i] = lines[i].Replace(ReadmeCallout,
                        ReadmeCallout.Replace(DefaultPassword,
                            RandomOpenIddictPassword));
                }

                if (lines[i].Contains(ProductionEncryptionAndSigningCertificate))
                {
                    lines[i] = lines[i].Replace(ProductionEncryptionAndSigningCertificate,
                        ProductionEncryptionAndSigningCertificate.Replace(DefaultPassword,
                            RandomOpenIddictPassword));

                    module = file.Name;
                }
            }

            file.SetLines(lines);
        }

        if (!module.IsNullOrWhiteSpace())
        {
            context.BuildArgs.ExtraProperties[nameof(RandomizeAuthServerPassPhraseStep)] = module;
        }
    }
}
