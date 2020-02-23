function AjaxLoader(base_box, controller, resultBox) {
    $(base_box).click(function () {
        $.ajax({
            type: "POST",
            url: "/" + controller + "/SetPagination",
            data: { page: 0 },
            success: function (result) {
                $(resultBox).html(result);
            }
        });
    });
}


class TableControllerContext {
    constructor(nameController, nameResultBox,removeFunction) {
        this.nameController = nameController;
        this.nameResultBox = nameResultBox;
        this.removeFunction = removeFunction;

        this.SetAjaxLoader();
        this.SetPagination();
        this.SetRemove();
    }

    SetAjaxLoader(nameController = this.nameController, nameResultBox = this.nameResultBox) {
        AjaxLoader("#" + nameController+"AjaxLoader", nameController, nameResultBox);
    }

    SetRemove(removeFunction = this.removeFunction ,nameController = this.nameController, nameResultBox = this.nameResultBox) {
        removeFunction(nameResultBox, "RemoveTable", nameController);
    }

    SetPagination(nameController = this.nameController, nameResultBox = this.nameResultBox) {
        Pagination(nameController, nameResultBox);
    }
}

/*AjaxLoader("#AuthorAjaxLoader", "Author", "#result-author-228");
AjaxLoader("#BookAjaxLoader", "Book", "#result-book-228");
AjaxLoader("#PublisherAjaxLoader", "Publisher", "#result-publisher-228");


Remove("#result-author-228", "RemoveTable", "Author");
Remove("#result-book-228", "RemoveTable", "Book");
Remove("#result-publisher-228", "RemoveTable", "Publisher");

fun("Search", ".box-genre-index", "#search-228-author", "#result-author-228");
funBook(".box-genre-index2", "#search-228-author2", ".box-genre-index3", "#search-author-book-228", "#result-book-228");
funPublisher("#search-228-publisher", "#search-228-publisher-book", "#result-publisher-228");

Pagination("Author", "#result-author-228");
Pagination("Book", "#result-book-228");
Pagination("Publisher", "#result-publisher-228")*/