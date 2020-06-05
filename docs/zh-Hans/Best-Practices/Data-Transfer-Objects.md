## 数据传输对象最佳实践 & 约定

* **推荐** 在 **application.contracts** 层中定义DTO.
* **推荐** 在可能和必要的情况下从预构建的 **基础DTO类** 继承 (如 `EntityDto<TKey>`, `CreationAuditedEntityDto<TKey>`, `AuditedEntityDto<TKey>`, `FullAuditedEntityDto<TKey>` 等).
* **推荐** 从**聚合根**的**扩展DTO**继承(如 `ExtensibleAuditedEntityDto<TKey>`), 因为聚合根是可扩展的额外的属性使用这种方式映射到DTO.
* **推荐** 定义 **public getter 和 setter** 的DTO成员 .
* **推荐** 使用 **data annotations** **验证** service输入DTO的属性.
* **不推荐** 在DTO中添加任何 **逻辑**, 在必要的时候可以实现  `IValidatableObject` 接口.
* **推荐** 为所有的DTO标记 **[Serializable]** Attribute. 因为它们已经是可序列化的, 开发人员可能会希望进行二进制序列化.
