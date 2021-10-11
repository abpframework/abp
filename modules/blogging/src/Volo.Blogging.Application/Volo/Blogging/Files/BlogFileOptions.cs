using System;

namespace Volo.Blogging.Files
{
    /* TODO:
     * - It is not to have different options for all different modules. We should find a more generic way.
     * - Actually, it is not good to assume to save to a local folder. Instead, use file storage once implemented.
     */
    public class BlogFileOptions
    {
        [Obsolete("Now store using blob storing system, will be deleted in a next version")]
        public string FileUploadLocalFolder { get; set; }
    }
}
