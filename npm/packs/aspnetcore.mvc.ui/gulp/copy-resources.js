"use strict";

(function () {

    var gulp = require("gulp"),
        merge = require("merge-stream"),
        fs = require('fs'),
        glob = require('glob'),
        micromatch = require('micromatch'),
        path = require("path"),
        extendObject = require('extend-object');

    var investigatedPackagePaths = {};

    var resourceMapping;
    var rootPath;

    function replaceAliases(text) {
        if (!resourceMapping.aliases) {
            return text;
        }

        for (var alias in resourceMapping.aliases) {
            if (!resourceMapping.aliases.hasOwnProperty(alias)) {
                continue;
            }

            text = replaceAll(text, alias, resourceMapping.aliases[alias]);
        }

        return text;
    }

    function replaceAll(text, search, replacement) {
        return text.replace(new RegExp(search, 'g'), replacement);
    }

    function requireOptional(filePath) {
        //TODO: Implement this using a library instead of try-catch!
        try {
            return require(filePath);
        } catch (e) {
            return undefined;
        } 
    }

    function cleanDirsAndFiles(patterns) {
        const { dirs, files } = findDirsAndFiles(patterns);

        files.forEach(file => {
            try {
                fs.unlinkSync(file);
            } catch (_) {}
        });

        dirs.sort((a, b) => a < b ? 1 : -1);

        dirs.forEach(dir => {
            if (fs.readdirSync(dir).length) return;

            try {
                fs.rmdirSync(dir, {});
            } catch (_) {}
        });
    }

    function findDirsAndFiles(patterns) {
        const dirs = [];
        const files = [];

        const list = glob.sync('**/*', { dot: true });

        const matches = micromatch(list, normalizeGlob(patterns), {
            dot: true,
        });

        matches.forEach(match => {
            if (!fs.existsSync(match)) return;

            (fs.statSync(match).isDirectory() ? dirs : files).push(match);
        });

        return { dirs, files };
    }

    function normalizeGlob(patterns) {
        return patterns.map(pattern => {
            const prefix = /\*$/.test(pattern) ? '' : '/**';
            return replaceAliases(pattern).replace(/(!?)\.\//, '$1') + prefix;
        });
    }
    
    function normalizeResourceMapping(resourcemapping) {
        var defaultSettings = {
            aliases: {
                "@node_modules": "./node_modules",
                "@libs": "./wwwroot/libs"
            },
            clean: [
                "@libs"
            ]
        };
        
        extendObject(defaultSettings.aliases, resourcemapping.aliases);
        resourcemapping.aliases = defaultSettings.aliases;
        
        resourcemapping.clean = resourcemapping.clean || defaultSettings.clean;
        
        return resourcemapping;
    }

    function buildResourceMapping(packagePath) {
        if (investigatedPackagePaths[packagePath]) {
            return {};
        }

        investigatedPackagePaths[packagePath] = 'OK';

        var packageJson = requireOptional(path.join(packagePath, 'package.json'));
        var resourcemapping = requireOptional(path.join(packagePath, 'abp.resourcemapping.js')) || { };

        if (packageJson && packageJson.dependencies) {
            var aliases = {};
            var mappings = {};

            for (var dependency in packageJson.dependencies) {
                if (packageJson.dependencies.hasOwnProperty(dependency)) {
                    var dependedPackagePath = path.join(rootPath, 'node_modules', dependency);
                    var importedResourceMapping = buildResourceMapping(dependedPackagePath);
                    extendObject(aliases, importedResourceMapping.aliases);
                    extendObject(mappings, importedResourceMapping.mappings);
                }
            }

            extendObject(aliases, resourcemapping.aliases);
            extendObject(mappings, resourcemapping.mappings);

            resourcemapping.aliases = aliases;
            resourcemapping.mappings = mappings;
        }

        return resourcemapping;
    }

    function copyResourcesTask (path) {
        rootPath = path;
        resourceMapping = normalizeResourceMapping(buildResourceMapping(rootPath));
    
        cleanDirsAndFiles(resourceMapping.clean);

        var tasks = [];

        if (resourceMapping.mappings) {
            for (var mapping in resourceMapping.mappings) {
                if (resourceMapping.mappings.hasOwnProperty(mapping)) {
                    var destination = replaceAliases(resourceMapping.mappings[mapping]);
                    if (fs.existsSync(destination)) continue;

                    var source = replaceAliases(mapping);
                    tasks.push(
                        gulp.src(source).pipe(gulp.dest(destination))
                    );
                }
            }
        }

        return merge(tasks);
    }

    module.exports = copyResourcesTask;
    
})();