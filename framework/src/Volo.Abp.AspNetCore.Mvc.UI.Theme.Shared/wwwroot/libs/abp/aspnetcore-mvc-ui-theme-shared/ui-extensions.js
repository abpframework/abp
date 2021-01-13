var abp = abp || {};
(function () {
    abp.ui = abp.ui || {};
    abp.ui.extensions = abp.ui.extensions || {};

    abp.ui.extensions.ActionList = function () {
        return new abp.utils.common.LinkedList();
    };

    abp.ui.extensions.ColumnList = function () {
        return new abp.utils.common.LinkedList();
    };

    abp.ui.extensions.entityActions = (function () {
        var _callbackLists = {};

        function _get(name) {
            var callbackList = _callbackLists[name];

            if (!callbackList) {
                callbackList = _callbackLists[name] = [];
            }

            return {
                addContributor: _addContributor,
                get actions() {
                    return _getActions();
                }
            };

            function _addContributor(contributeCallback, order) {
                if (order === undefined || order >= callbackList.length) {
                    callbackList.push(contributeCallback);
                } else if (order === 0) {
                    callbackList.unshift(contributeCallback);
                } else {
                    callbackList.splice(order, 0, contributeCallback);
                }
            }

            function _getActions() {
                var actionList = new abp.ui.extensions.ActionList();

                callbackList.forEach(function (callback) {
                    callback(actionList);
                });

                return actionList;
            }
        }

        return {
            get: _get
        };
    })();

    abp.ui.extensions.tableColumns = (function () {
        var _callbackLists = {};

        function _get(name) {
            var callbackList = _callbackLists[name];

            if (!callbackList) {
                callbackList = _callbackLists[name] = [];
            }

            return {
                addContributor: _addContributor,
                get columns() {
                    return _getColumns();
                }
            };

            function _addContributor(contributeCallback, order) {
                if (order === undefined || order >= callbackList.length) {
                    callbackList.push(contributeCallback);
                } else if (order === 0) {
                    callbackList.unshift(contributeCallback);
                } else {
                    callbackList.splice(order, 0, contributeCallback);
                }
            }

            function _getColumns() {
                var columnList = new abp.ui.extensions.ColumnList();

                callbackList.forEach(function (callback) {
                    callback(columnList);
                });

                return columnList;
            }
        }

        return {
            get: _get
        };
    })();

    function initializeObjectExtensions() {

        var getShortEnumTypeName = function (enumType) {
            var lastDotIndex = enumType.lastIndexOf('.');
            if (lastDotIndex < 0) {
                return enumType;
            }

            return enumType.substr(lastDotIndex + 1);
        };

        var getEnumMemberName = function (enumInfo, enumMemberValue) {
            for (var i = 0; i < enumInfo.fields.length; i++) {
                var enumField = enumInfo.fields[i];
                if (enumField.value == enumMemberValue) {
                    return enumField.name;
                }
            }

            return null;
        };

        function localizeDisplayName(propertyName, displayName) {
            if (displayName && displayName.name) {
                return abp.localization.localize(displayName.name, displayName.resource);
            }

            if (abp.localization.isLocalized('DisplayName:' + propertyName)) {
                return abp.localization.localize('DisplayName:' + propertyName);
            }

            return abp.localization.localize(propertyName);
        }

        function localizeWithFallback(localizationResources, keys, defaultValue) {
            for (var i = 0; i < localizationResources.length; i++) {
                var localizationResource = localizationResources[i];
                if (!localizationResource) {
                    continue;
                }

                for (var j = 0; j < keys.length; j++) {
                    var key = keys[j];

                    if (abp.localization.isLocalized(key, localizationResource)) {
                        return abp.localization.localize(key, localizationResource);
                    }
                }
            }

            return defaultValue;
        }

        function localizeEnumMember(property, enumMemberValue) {
            var enumType = property.config.type;
            var enumInfo = abp.objectExtensions.enums[enumType];
            var enumMemberName = getEnumMemberName(enumInfo, enumMemberValue);

            if (!enumMemberName) {
                return enumMemberValue;
            }

            var shortEnumType = getShortEnumTypeName(enumType);

            return localizeWithFallback(
                [enumInfo.localizationResource, abp.localization.defaultResourceName],
                [
                    'Enum:' + shortEnumType + '.' + enumMemberName,
                    shortEnumType + '.' + enumMemberName,
                    enumMemberName
                ],
                enumMemberName
            );
        }

        function configureTableColumns(tableName, columnConfigs) {
            abp.ui.extensions.tableColumns.get(tableName)
                .addContributor(
                    function (columnList) {
                        columnList.addManyTail(columnConfigs);
                    }
                );
        }

        function getTableProperties(objectConfig) {
            var propertyNames = Object.keys(objectConfig.properties);
            var tableProperties = [];
            for (var i = 0; i < propertyNames.length; i++) {
                var propertyName = propertyNames[i];
                var propertyConfig = objectConfig.properties[propertyName];
                if (propertyConfig.ui.onTable.isVisible) {
                    if (propertyName.endsWith("_Text")) {
                        var lookupPropertyName = propertyName.replace("_Text", "");
                        var lookupProperty = objectConfig.properties[lookupPropertyName];
                        if (lookupProperty) {
                            tableProperties.push({
                                name: propertyName,
                                config: propertyConfig,
                                lookupPropertyName: lookupPropertyName,
                                lookupPropertyDisplayName: lookupProperty.displayName
                            });
                        } else {
                            tableProperties.push({
                                name: propertyName,
                                config: propertyConfig,
                            });
                        }
                    } else {
                        tableProperties.push({
                            name: propertyName,
                            config: propertyConfig,
                        });
                    }
                }
            }

            return tableProperties;
        }

        function getValueFromRow(property, row) {
            return row.extraProperties[property.name];
        }

        function convertPropertyToColumnConfig(property) {
            var columnConfig = {
                data: "extraProperties." + property.name,
                orderable: false
            };

            if (property.lookupPropertyName) {
                columnConfig.title = localizeDisplayName(property.lookupPropertyName, property.lookupPropertyDisplayName);
            } else {
                columnConfig.title = localizeDisplayName(property.name, property.config.displayName);
            }

            if (property.config.typeSimple === 'enum') {
                columnConfig.render = function (data, type, row) {
                    var value = getValueFromRow(property, row);
                    return localizeEnumMember(property, value);
                }
            } else {
                var defaultRenderer = abp.libs.datatables.defaultRenderers[property.config.typeSimple];
                if (defaultRenderer) {
                    columnConfig.render = function (data, type, row) {
                        var value = getValueFromRow(property, row);
                        return defaultRenderer(value);
                    }
                }
            }

            return columnConfig;
        }

        function convertPropertiesToColumnConfigs(properties) {
            var columnConfigs = [];

            for (var i = 0; i < properties.length; i++) {
                columnConfigs.push(convertPropertyToColumnConfig(properties[i]));
            }

            return columnConfigs;
        }

        function configureEntity(moduleName, entityName, entityConfig) {
            var tableProperties = getTableProperties(entityConfig);

            if (tableProperties.length > 0) {
                var tableName = abp.utils.toCamelCase(moduleName) + "." + abp.utils.toCamelCase(entityName);
                var columnConfigs = convertPropertiesToColumnConfigs(tableProperties);
                configureTableColumns(
                    tableName,
                    columnConfigs
                );
            }
        }

        function configureModule(moduleName, moduleConfig) {
            var entityNames = Object.keys(moduleConfig.entities);
            for (var i = 0; i < entityNames.length; i++) {
                configureEntity(
                    moduleName,
                    entityNames[i],
                    moduleConfig.entities[entityNames[i]]
                );
            }
        }

        var moduleNames = Object.keys(abp.objectExtensions.modules);

        for (var i = 0; i < moduleNames.length; i++) {
            configureModule(
                moduleNames[i],
                abp.objectExtensions.modules[moduleNames[i]]
            );
        }
    }

    abp.event.on('abp.configurationInitialized', function () {
        initializeObjectExtensions();
    });

})();
