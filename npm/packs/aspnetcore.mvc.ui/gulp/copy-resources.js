"use strict";

(function () {

    var taskName = 'aspnetcore.mvc.ui.copy-resources';

    var gulp = require("gulp"),
        merge = require("merge-stream"),
        rimraf = require("rimraf"),
        path = require("path"),
        extendObject = require('extend-object');

    function init(rootPath) {
        var investigatedPackagePaths = {};

        var resourceMapping = buildResourceMapping(rootPath);

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
        
        function cleanFiles() {
            if (resourceMapping.clean) {
                for (var i = 0; i < resourceMapping.clean.length; i++) {
                    rimraf.sync(replaceAliases(resourceMapping.clean[i]) + '/**/*', { force: true });
                }
            }
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

        gulp.task(taskName,
            function () {
                cleanFiles();

                var tasks = [];

                if (resourceMapping.mappings) {
                    for (var mapping in resourceMapping.mappings) {
                        if (resourceMapping.mappings.hasOwnProperty(mapping)) {
                            var source = replaceAliases(mapping);
                            var destination = replaceAliases(resourceMapping.mappings[mapping]);
                            tasks.push(
                                gulp.src(source).pipe(gulp.dest(destination))
                            );
                        }
                    }
                }

                return merge(tasks);
            });
    }

    module.exports = {
        taskName: taskName,
        init: init
    };
})();