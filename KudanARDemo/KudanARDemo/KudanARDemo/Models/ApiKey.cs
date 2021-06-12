using System;
using System.Collections.Generic;
using System.Text;

namespace KudanARDemo.Models
{
    public class ApiKey
    {
#if true
        public static readonly string AppSyncApiUrl = "";
        public static readonly string AppSyncApiKey = "";

        public static string KudanARApiKey { get; set; }
#else

        // 下記パッケージ名使用時のAPIキー(有効期限 2021/01/01)
        // com.xlsoft.kudanar
        public static readonly string KudanARApiKey = "QDJFL+/L+a6KPmYOyGKdmxsaW45VNzMNWuP7ovEZMFzuBcw6IwDglo+a2pOwOV60bEMfNfEAFtmv/AaKWDGfC3V93MwJk3o74tC2Lh4OFokEWXqJmGGwrx7hAayiUdOGpz37ptsvlXHdap9Nl6cMGPe+cc2sVxBpGO60z5QES8VQvA3k7SnMNm0sYO1gJCb+Ryx/3EOuPHr8C506WbOjIqxrCWXOdp+wnQUQ4kZVI6KnXWEiZUu9Hr9/61PPEy+l62lgBneO0bFwTH6yriz+JuFaZIQW6NQmSEuin40HbVHpLJKPqWWVwOuocgExMkv7FBtfeonCM/zHGzxGRCXTlfPKQiVi5O3q5ArLXP1SJEFM0c4XqSREUcUnBQv6sqNht8nBPkg/qAeZGiIceIvoA6w8yTWV6BfBrnF76kbmgtkdNQ1XikmA73CsA3/upcxcpL4Rp2is4ZQyZvNgbZO7fvhqEIKbb0Cixm3b8uL6G45OQHyOYQBh+AolvOXUmQrYPZgXGXlWGPWPKJTrZg4rWrwceHvn+yGdtnaWCuLghL2BKBBziWb0d9AnQ8xgS16JNDlxVdFjZgucimffdDKFCJfpFVzktCPt6rQLGsuU6BUvpdImPnHOlL/pFX34IQPqZZ7Y2XrV+XxHaoJi3bWym5YESmUsrJkhAwKELv0obU0=";
#endif
    }
}
