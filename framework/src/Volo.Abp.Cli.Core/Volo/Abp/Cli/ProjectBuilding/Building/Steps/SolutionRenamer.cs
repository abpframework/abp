using System;
using System.Collections.Generic;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class SolutionRenamer
{
    private readonly List<FileEntry> _entries;
    private readonly string _companyNamePlaceHolder;
    private readonly string _projectNamePlaceHolder;

    private readonly string _companyName;
    private readonly string _projectName;

    public SolutionRenamer(List<FileEntry> entries, string companyNamePlaceHolder, string projectNamePlaceHolder, string companyName, string projectName)
    {
        if (string.IsNullOrWhiteSpace(companyName))
        {
            companyName = null;
        }

        if (companyNamePlaceHolder == null && companyName != null)
        {
            throw new UserFriendlyException($"Can not set {nameof(companyName)} if {nameof(companyNamePlaceHolder)} is null.");
        }

        _entries = entries;

        _companyNamePlaceHolder = companyNamePlaceHolder;
        _projectNamePlaceHolder = projectNamePlaceHolder ?? throw new ArgumentNullException(nameof(projectNamePlaceHolder));

        _companyName = companyName;
        _projectName = projectName ?? throw new ArgumentNullException(nameof(projectName));
    }

    public void Run()
    {
        if (_companyNamePlaceHolder != null)
        {
            if (_companyName != null)
            {
                RenameHelper.RenameAll(_entries, _companyNamePlaceHolder, _companyName);
                RenameHelper.RenameAll(_entries, _companyNamePlaceHolder.ToCamelCase(), _companyName.ToCamelCase());
                RenameHelper.RenameAll(_entries, _companyNamePlaceHolder.ToKebabCase(), _companyName.ToKebabCase());
                RenameHelper.RenameAll(_entries, _companyNamePlaceHolder.ToLowerInvariant(), _companyName.ToLowerInvariant());
            }
            else
            {
                RenameHelper.RenameAll(_entries, _companyNamePlaceHolder + "." + _projectNamePlaceHolder, _projectNamePlaceHolder);
                RenameHelper.RenameAll(_entries, _companyNamePlaceHolder.ToCamelCase() + "." + _projectNamePlaceHolder.ToCamelCase(), _projectNamePlaceHolder.ToCamelCase());
                RenameHelper.RenameAll(_entries, _companyNamePlaceHolder.ToLowerInvariant() + "." + _projectNamePlaceHolder.ToLowerInvariant(), _projectNamePlaceHolder.ToLowerInvariant());
                RenameHelper.RenameAll(_entries, _companyNamePlaceHolder.ToKebabCase() + "/" + _projectNamePlaceHolder.ToKebabCase(), _projectNamePlaceHolder.ToKebabCase());
            }
        }

        RenameHelper.RenameAll(_entries, _projectNamePlaceHolder, _projectName);
        RenameHelper.RenameAll(_entries, _projectNamePlaceHolder.ToCamelCase(), _projectName.ToCamelCase());
        RenameHelper.RenameAll(_entries, _projectNamePlaceHolder.ToKebabCase(), _projectName.ToKebabCase());
        RenameHelper.RenameAll(_entries, _projectNamePlaceHolder.ToLowerInvariant(), _projectName.ToLowerInvariant());
        RenameHelper.RenameAll(_entries, _projectNamePlaceHolder.ToSnakeCase().ToUpper(), _projectName.ToSnakeCase().ToUpper());
    }
}