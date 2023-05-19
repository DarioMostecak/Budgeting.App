// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

namespace Budgeting.Web.App.Models.Categories
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModify { get; set; }
    }
}
