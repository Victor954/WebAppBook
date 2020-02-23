var GlobalId;

var sel;

var costil;

function Genre(Obj) {
    sel = 0;
    return { Id: Obj.Id, GenreName: Obj.Value };
}
function Language(Obj) {
    sel = 1;
    return { Id: Obj.Id, LanguageName: Obj.Value };
}

class ControllList {
    constructor(controller, parent_elem, insert_text) {
        this.controller = controller;
        this.parent_elem = parent_elem;
        this.insert_text = insert_text;
    }

    Obj(Id, Name) {
        return { Id: Id, Value: Name };
    }

    SetRemoveList(fun = Genre ,controller = this.controller, parent_elem = this.parent_elem,ajax = this.Obj()) {
        $(parent_elem +" .box-fow").on("click", '.close-m', function () {
            $(parent_elem + " select").append("<option>" + $(this).next().html() + "</option>")

            var val = $(this).siblings('.val-genre').html();
            var id = $(this).parent().attr("id");

            ajax.Id = id;
            ajax.Value = val;

            $.ajax({
                type: "POST",
                dataType: 'json',
                url: '/'+controller+'/RemoveList',
                data: fun(ajax)
            });

            $(this).parent().remove();

        });
    }

    SetAddList(insert_text = this.insert_text, fun = Genre, controller = this.controller, parent_elem = this.parent_elem, ajax = this.Obj()) {
        $(parent_elem + " select").change(function () {
            var val = $(this).val();
            var id = $(this).find("option:contains('" + val + "')").attr("id");

            ajax.Id = id;
            ajax.Value = val;

            if (val != insert_text) {

                $(parent_elem + " .box-fow").append("<span class='target-genre' id='" + id + "'><span class='close-m'>x</span><span class='val-genre'>" + val + "</span></span>")
                $(this).find("option:contains('" + val + "')").remove();
                $.ajax({
                    type: "POST",
                    url: '/' + controller + '/AddList',
                    dataType: 'json',
                    data: fun(ajax)
                });
            }
        });
    }
}

class ControllTable {
    constructor(controller, parent_elem, parent_elem2, insert_text, is_val = true,obj = Genre) {
        this.controller = controller;
        this.parent_elem = parent_elem;
        this.parent_elem2 = parent_elem2;
        this.array_element = new Array();
        this.controll_list = new ControllList(controller, parent_elem2);

        this.insert_text = insert_text;

        this.is_val = is_val;

        this.SetEdit(obj);
        this.SetAdd(obj);
        this.SetRemove(obj);
        this.SetSearch(obj);
    }

    Obj(Id, Name) {
        return { Id: Id, Value: Name };
    }

    SetEdit(fun = Genre, ajax = this.Obj(), controller = this.controller, is = this.is_val, elem = this.parent_elem, Res = this.Res, elem2 = this.parent_elem2, insert_text = this.insert_text) {
        $(this.parent_elem).on("click", ".btn-edit-call", function () {
            var Id = $(this).parent().attr("id");
            var Name = $(this).siblings(".form-control").val();

            ajax.Id = Id;
            ajax.Value = Name;

            if (is) {
                Res(
                    '/' + controller + '/EditDb',
                    fun(ajax), elem2, elem, insert_text);
            }
            else {
                ResMinDynamic(
                    '/' + controller + '/EditDb',
                    fun(ajax), $(this).parent());
            }

        });
    }
    SetAdd(fun = Genre, ajax = this.Obj(), controller = this.controller, is = this.is_val, elem = this.parent_elem, Res = this.Res, elem2 = this.parent_elem2, insert_text = this.insert_text) {
        $(this.parent_elem).on("click", ".add-genre-228", function () {

            var val = $(this).siblings(".add-genre-form").val();
            ajax.Value = val;
            if (is) {
                Res(
                    '/' + controller + '/AddDb',
                    fun(ajax), elem2, elem, insert_text);
            }
            else {
                ResMin(
                    '/' + controller + '/AddDb',
                    fun(ajax), elem);
            }
        });
    }
    SetRemove(fun = Genre, ajax = this.Obj(), controller = this.controller, is = this.is_val, elem = this.parent_elem, Res = this.Res, elem2 = this.parent_elem2, insert_text = this.insert_text) {
        $(this.parent_elem).on("click", ".btn-delete-call", function () {
            var Id = $(this).parent().attr("id");
            var Name = $(this).siblings(".form-control").val();

            GlobalId = Id;
            ajax.Id = Id;
            ajax.Value = Name;

            if (is) {
                Res(
                    '/' + controller + '/DeleteDb',
                    fun(ajax), elem2, elem, insert_text);
            }
            else {
                ResMinDynamicDelete(
                    '/' + controller + '/DeleteDb',
                    fun(ajax), $(this).parent());
            }
        });
    }
    SetSearch(fun = Genre,controller = this.controller, elem = this.parent_elem) {
        $(elem + " .add-genre-form-228").keyup(function () {
            fun({ Id: 0, Value:0});

            ResMin('/' + controller + '/Search',
                { 'search': $(this).val() }, elem);
        });
    }

    Res(url, data, elem, elem2, insert_text) {

    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: 'json',
        success: function (result) {
            $(elem2 + " .box-fow").html("");
            $(elem + " select").html("");

            $(elem + " select").append("<option>" + insert_text +"</option>");
            var val = $(elem + " .box-fow"+" .val-genre");

            for (var i = 0; i < result.length; i++) {
                var va = (sel == 0) ? result[i].GenreName : result[i].LanguageName;
                $(elem2 + " .box-fow").append(Content(result[i].Id, va));
                $(elem2 + " .box-fow").children().eq(i).stop().animate({ "opacity": 1 }, i * 250);

                for (var z = 0; z < val.length; z++) {
                    var v = $(elem + " .box-fow"+" .val-genre").eq(z).html();
                    var id = $(elem + " .box-fow"+" .target-genre").eq(z).attr("id");

                    if (id == result[i].Id && va != v) {
                        $(elem + " .box-fow" + " .val-genre").eq(z).html(va);
                    }

                    if (id == GlobalId) {
                        $(elem + " .box-fow"+" .target-genre").eq(z).remove();
                    }

                }

                $(elem + " select").append("<option id='" + result[i].Id + "'>" + va + "</option>");
            }

            for (var y = 0; y < val.length; y++) {
                $(elem + " select" + ' option[id = ' + $(elem + " .box-fow" + " .target-genre").eq(y).attr("id") +"]").remove();
            }
        }
    });
}
}
var arr = new Array();
function Content(Id, Name) {
    return "<div class='input-group input-group228' id='" + Id + "' style='opacity:0'>"
        + "<input type='text' name='LastName' value='" + Name + "' class='form-control' placeholder='Название жанра' />"
        + "<button id='add' class='btn btn-form btn-edit btn-edit-call'>С</button>"
        + "<button id='add' class='btn btn-form btn-delete-call'>У</button></div>"
}

function Content2(Id, Name, LastName, PatronymicName) {

    return "<div class='input-group input-group228' id='" + Id +"' style='opacity:0'>"
        + "<input type='text' name='Name' value='" + Name + "' class='form-control' placeholder='Имя' />"
        + "<input type='text' name='LastName' value='" + LastName + "' class='form-control' placeholder='Фамилия' />"
        + "<input type='text' name='PatronymicName' value='" + PatronymicName+"' class='form-control' placeholder='Отчество' />"
        +"<button id='add' class='btn btn-form btn-edit btn-edit-call'>С</button>"
        +"<button id='add' class='btn btn-form btn-delete-call'>У</button>"
    +"</div>"
}

function ResMin(url, data, elem) {
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: 'json',
        success: function (result) {
            $(elem+" .box-fow").html("");

            for (var i = 0; i < result.length; i++) {
                var test = (sel == 0) ? result[i].GenreName : result[i].LanguageName;
                $(elem + " .box-fow").append(Content(result[i].Id, test));
                $(elem + " .box-fow").children().eq(i).stop().animate({ "opacity": 1 }, i * 250);
            }
        }
    });
}

function ResMinDynamic(url, data, elem) {
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: 'json',
        success: function (result) {
            $(elem).stop().animate({ "opacity": 0 }, 300).animate({ "opacity": 1 }, 300);
        }
    });
}

function ResMinDynamicDelete(url, data, elem) {
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: 'json',
        success: function (result) {
            $(elem).css({ "overflow":"hidden"}).stop().animate({ "opacity": 0 ,"height" : 0 },300, function () {
                $(elem).remove();
            });
        }
    });
}

function Request(url, data) {
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        dataType: 'json',
        success: function (result) {
            $(".box-con3 .box-fow").html("");

            for (var i = 0; i < result.length; i++) {
                $(".box-con3 .box-fow").append(Content2(result[i].Id, result[i].Name, result[i].LastName, result[i].PatronymicName));
                $(".box-con3 .box-fow").children().eq(i).stop().animate({ "opacity": 1 }, i * 250)
            }
        }
    });
}


class ControllListTranslator {
    constructor(controller, parent_elem, insert_text) {
        this.controller = controller;
        this.parent_elem = parent_elem;
        this.insert_text = insert_text;
    }

    Obj(Id, Name) {
        return { Id: Id, Value: Name };
    }

    SetRemoveList(controller = this.controller, parent_elem = this.parent_elem, ajax = this.Obj()) {
        $(parent_elem + " .box-fow").on("click", '.close-m', function () {
            $(parent_elem + " select").append("<option>" + $(this).next().html() + "</option>")

            var val = $(this).siblings('.val-genre').html();
            var id = $(this).parent().attr("id");

            var ar = val.split(' ');

            $.ajax({
                type: "POST",
                dataType: 'json',
                url: '/' + controller + '/RemoveList',
                data: ({ Id: id, Name: ar[0], LastName: ar[1], PatronymicName: ar[2] })
            });

            $(this).parent().remove();

        });
    }

    SetAddList(insert_text = this.insert_text, controller = this.controller, parent_elem = this.parent_elem) {
        $(parent_elem + " select").change(function () {
            var val = $(this).val();
            var id = $(this).find("option:contains('" + val + "')").attr("id");

            var ar = val.split(' ');

            if (val != insert_text) {

                $(parent_elem + " .box-fow").append("<span class='target-genre' id='" + id + "'><span class='close-m'>x</span><span class='val-genre'>" + val + "</span></span>")
                $(this).find("option:contains('" + val + "')").remove();
                $.ajax({
                    type: "POST",
                    url: '/' + controller + '/AddList',
                    dataType: 'json',
                    data: ({Id:id, Name: ar[0], LastName: ar[1], PatronymicName: ar[2]})
                });
            }
        });
    }
}

class ControllTableTranslator {
    constructor(parent_elem, parent_elem2, insert_text, is_val = true) {
        this.parent_elem = parent_elem;
        this.parent_elem2 = parent_elem2;
        this.insert_text = insert_text;

        this.controll_list = new ControllListTranslator("Translator", parent_elem2, insert_text);

        this.is_val = is_val;

        this.SetEdit();
        this.SetAdd();
        this.SetRemove();
        this.SetSearch();
    }

    Obj(Id, Name) {
        return { Id: Id, Value: Name };
    }

    SetEdit(insert_text = this.insert_text,parent_elem = this.parent_elem, parent_elem2 = this.parent_elem2, is_val = this.is_val, Res = this.Res) {
        $(parent_elem).on("click", ".btn-edit-call", function () {
            var Name = $(this).siblings("input").eq(0).val();
            var LastName = $(this).siblings("input").eq(1).val();
            var PatronymicName = $(this).siblings("input").eq(2).val();
            var Id = $(this).parent().attr("id");

            if (is_val) {
                Res("/Translator/EditDb",
                    { Id: Id, Name: Name, LastName: LastName, PatronymicName: PatronymicName },
                    parent_elem2,
                    parent_elem,
                    insert_text
                )
            }
            else {
                ResMinDynamic("/Translator/EditDb", { Id: Id, Name: Name, LastName: LastName, PatronymicName: PatronymicName }, $(this).parent());
            }
        });
    }

    SetAdd(parent_elem = this.parent_elem, is_val = this.is_val) {
        $(parent_elem + " .add-genre-228").click(function () {
            var val = $(parent_elem + " .add-genre-form").val();
            var ar = val.split(' ');

            if (ar.length == 3) {
                if (is_val) {
                    Res("/Translator/EditDb",
                        { Id: Id, Name: Name, LastName: LastName, PatronymicName: PatronymicName },
                        parent_elem2,
                        parent_elem,
                        insert_text
                    )
                }
                else {
                    Request("/Translator/AddDb", { Name: ar[0], LastName: ar[1], PatronymicName: ar[2] });
                }
            }

        });
    }

    SetRemove(parent_elem = this.parent_elem, is_val = this.is_val) {
        $(parent_elem).on("click", ".btn-delete-call", function () {
            var Id = $(this).parent().attr("id");

            if (is_val) {
                Res("/Translator/EditDb",
                    { Id: Id, Name: Name, LastName: LastName, PatronymicName: PatronymicName },
                    parent_elem2,
                    parent_elem,
                    insert_text
                )
            }
            else {
                ResMinDynamicDelete("/Translator/DeleteDb", { Id: Id }, $(this).parent());
            }
        });
    }

    SetSearch(parent_elem = this.parent_elem) {
        $(parent_elem + " .add-genre-form-228").keyup(function () {
            var search = $(this).val();

            Request("/Translator/Search", { search: search });
        });
    }

    Res(url, data, elem, elem2, insert_text) {

        $.ajax({
            type: "POST",
            url: url,
            data: data,
            dataType: 'json',
            success: function (result) {
                $(elem2 + " .box-fow").html("");
                $(elem + " select").html("");

                $(elem + " select").append("<option>" + insert_text + "</option>");
                var val = $(elem + " .box-fow" + " .val-genre");

                for (var i = 0; i < result.length; i++) {
                    var va = result[i].Name + " " + result[i].LastName + " " + result[i].PatronymicName;

                    $(elem2 + " .box-fow").append(Content2(result[i].Id, result[i].Name, result[i].LastName, result[i].PatronymicName));

                    for (var z = 0; z < val.length; z++) {
                        var v = $(elem + " .box-fow" + " .val-genre").eq(z).html();
                        var id = $(elem + " .box-fow" + " .target-genre").eq(z).attr("id");

                        if (id == result[i].Id && va != v) {
                            $(elem + " .box-fow" + " .val-genre").eq(z).html(va);
                        }

                        if (id == GlobalId) {
                            $(elem + " .box-fow" + " .target-genre").eq(z).remove();
                        }

                    }

                    $(elem + " select").append("<option id='" + result[i].Id + "'>" + va + "</option>");
                }

                for (var y = 0; y < val.length; y++) {
                    $(elem + " select" + ' option[id = ' + $(elem + " .box-fow" + " .target-genre").eq(y).attr("id") + "]").remove();
                }
            }
        });
    }
}

class LoadImage {
    constructor(ControllerName,MethodName) {
        this.MethodName = MethodName;
        this.ControllerName = ControllerName;

        this.Load(MethodName);
    }

    Load(MethodName = this.MethodName, ControllerName = this.ControllerName) {
        $("#document-img").change(function () {

            var files = document.getElementById('document-img').files;
            if (files.length > 0) {
                if (window.FormData !== undefined) {
                    var data = new FormData();
                    for (var x = 0; x < files.length; x++) {
                        data.append("file" + x, files[x]);
                    }

                    $.ajax({
                        type: "POST",
                        url: "/" + ControllerName+"/" + MethodName,
                        contentType: false,
                        processData: false,
                        data: data,
                        success: function (result) {
                            $("#document-img-src").val(result);
                        },
                    });
                } else {
                    alert("Браузер не поддерживает загрузку файлов HTML5!");
                }
            }
        });
    }
}


$(".btn-hamburger").click(function () {
    $(".box-menu").toggleClass("hide-menu");
});

$("#addAuthor").click(function () {
    $("#zalu").click();
});

$("#click-form-img").click(function () {
    $("#document-img").click();
});

$("#document-img").change(function () {
    var tgt = $("#document-img").target || window.event.srcElement,
        files = tgt.files;

    if (FileReader && files && files.length) {
        var fr = new FileReader();
        fr.onload = function () {
            document.getElementById("blah").src = fr.result;
        }
        fr.readAsDataURL(files[0]);
    }
});

function base() {
    $(".result-msg").stop().animate({ "top": "1rem" }, 500);
    $(".result-msg p").html("Sucsess!");

    setTimeout(function () { $(".result-msg").stop().animate({ "top": "-12rem" }, 500); }, 2000)
}
function result() {
    $(".result-msg").removeClass("error-msg");
    base();
}
function error() {
    $(".result-msg").addClass("error-msg");
    base();
}

$(document).ready(function () {
    $(".result-msg .close-msg").click(function () {
        $(".result-msg").stop().animate({ "top": "-12rem" }, 500);
    });
});

$(".filter").click(function () {
    var filter = $($(this).attr("data-target"));

    if (filter.hasClass("hide-228")) {
        filter.toggleClass("hide-228");

        $($(this).attr("data-target")).stop().animate({
            "height": "39rem",
            "padding-top": "2rem",
            "padding-bottom": "3rem"
        }, 500);
    }
});

$(".close-filter").click(function () {

    $($(this).attr("data-target")).stop().animate({
        "height": 0,
        "padding-top": 0,
        "padding-bottom": 0
    }, 500, function () {
        $(this).toggleClass("hide-228");
    });
});


function fun(Mthod, BaseBox, SearchBox, ResultBox) {
    var arr = new Array();
    $(BaseBox + " .click-add-index-228").click(function () {
        if ($(this).hasClass("Added")) {
            $(this).removeClass("Added");
            Many();
        }
        else {
            $(this).addClass("Added");

            Many();
        }
    });

    $(SearchBox).keyup(function (e) {
        Author(true,arr);
    });

    function Author(list = false, li = null) {
        var data = (list) ? { search: $(SearchBox).val(), listMany: li } : { search: $(SearchBox).val() };

        $.ajax({
            type: "POST",
            url: "/AuthorSearch/" + Mthod,
            data: data,
            success: function (result) {
                $(ResultBox).html(result);
            }
        });
    }

    function Many() {
        arr = new Array();
        for (var i = 0; i < $(BaseBox + " .Added").length; i++) {
            var obj = $(BaseBox + " .Added").eq(i);

            var id = obj.attr('id');
            var val = obj.html();

            arr.push({ Id: id, GenreName: val });
        }

        Author(true, arr);
    }
}

function funBook(BaseBox, SearchBox, BaseBox2, SearchBox2, ResultBox) {
    var arr = new Array();
    var arr2 = new Array();
    $(BaseBox + " .click-add-index-228").click(function () {
        if ($(this).hasClass("Added")) {
            $(this).removeClass("Added");
            Many();
        }
        else {
            $(this).addClass("Added");

            Many();
        }
    });

    $(BaseBox2 + " .click-add-index-228").click(function () {
        if ($(this).hasClass("Added")) {
            $(this).removeClass("Added");
            Many2();
        }
        else {
            $(this).addClass("Added");

            Many2();
        }
    });

    $(SearchBox).keyup(function (e) {
        Author();
    });
    $(SearchBox2).keyup(function (e) {
        Author();
    });

    function Book() {
        var lastReq = { search: $(SearchBox).val(), searchAuthor: $(SearchBox2).val(), }

        $.ajax({
            type: "POST",
            url: "/BookSearch/SearchBookSearh",
            data: lastReq,
            success: function (result) {
                $(ResultBox).html(result);
            }
        });
    }

    function Author() {

        var firstReq = { listMany: arr, listManyLanguage: arr2 };

        $.ajax({
            type: "POST",
            url: "/BookSearch/SearchBook",
            data: firstReq,
            success: function (result) {
                $(ResultBox).html(result);
            }
        });

        Book();
    }

    function Many() {
        arr = new Array();
        for (var i = 0; i < $(BaseBox + " .Added").length; i++) {
            let obj = $(BaseBox + " .Added").eq(i);


            let id = obj.attr('id');
            let val = obj.html();

            arr.push({ Id: id, GenreName: val });
        }
        Author();
    }

    function Many2() {
        arr2 = new Array();

        for (var f = 0; f < $(BaseBox2 + " .Added").length; f++) {
            let obj = $(BaseBox2 + " .Added").eq(f);

            let id = obj.attr('id');
            let val = obj.html();

            arr2.push({ Id: id, LanguageName: val });


        }
        Author();
    }
}

function funPublisher(SearchBox, SearchBox2, ResultBox) {
    $(SearchBox).keyup(function (e) {
        Publisher();
    });
    $(SearchBox2).keyup(function (e) {
        Publisher();
    });

    function Publisher() {
        var firstReq = { search: $(SearchBox).val(), searchBook: $(SearchBox2).val() };

        $.ajax({
            type: "POST",
            url: "/PublisherSearch/SearchPublisher",
            data: firstReq,
            success: function (result) {
                $(ResultBox).html(result);
            }
        });
    }
}

function Pagination(_Controller, _BaseBox) {
    var Controller = _Controller;
    var BaseBox = _BaseBox;

    GetByNumber(".pag-elem");
    GetByNumber(".last");
    GetByNumber(".start");

    function GetByNumber(elem) {
        $(BaseBox).on("click", elem, function () {

            var num = Number.parseInt($(this).html()) - 1;

            $.ajax({
                type: "POST",
                url: "/" + Controller + "/SetPagination",
                data: { page: num },
                success: function (result) {
                    $(BaseBox).html(result);
                }
            });
        });
    }
    $(BaseBox).on("click", ".next", function () {
        var curr = Number.parseInt($(this).siblings(".pagination-elems").children(".currect").html());
        var last = Number.parseInt($(this).siblings(".last").html());

        var num = (curr - 1) + 1;

        if (curr == last) {
            num = 0;
        }

        $.ajax({
            type: "POST",
            url: "/" + Controller + "/SetPagination",
            data: { page: num },
            success: function (result) {
                $(BaseBox).html(result);
            }
        });
    });
    $(BaseBox).on("click", ".prev", function () {
        var curr = Number.parseInt($(this).siblings(".pagination-elems").children(".currect").html()) - 1;
        var last = Number.parseInt($(this).siblings(".last").html());

        var num = curr - 1;

        if (curr == 0) {
            num = last - 1;
        }

        $.ajax({
            type: "POST",
            url: "/" + Controller + "/SetPagination",
            data: { page: num },
            success: function (result) {
                $(BaseBox).html(result);
            }
        });
    });
}