
function SetFind(arrVal ,funcSend)
{
    result = true;

    for (i = 0; i < arrVal.length; i++)
    {
        isUnEmpty = !validator.isEmpty(arrVal[i].val(), { ignore_whitespace: true });
        isUnNumber = !validator.isDecimal(arrVal[i].val());

        var baseRes = funcSend(arrVal[i]);

        result = (!result) ? false : baseRes;
        colorResult = baseRes;

        arrVal[i].attr("style", "");

        if (!colorResult) {
            arrVal[i].css({ borderColor:"Red" });
        }
    }

    return result;
}

function isEmptyAndDecimal(elem) {
    isUnEmpty = !validator.isEmpty(elem.val(), { ignore_whitespace: true });
    isUnNumber = !validator.isDecimal(elem.val());

    return (isUnEmpty && isUnNumber);
}

function isSetCountry(elem) {
    validateEquals = !validator.equals(elem.val(), "Выберите страну");

    return validateEquals;
}

function isSetCustom(elem,stringEquals) {
    validateEquals = !validator.equals(elem.val(), stringEquals);

    return validateEquals;
}

function isSetEmpty(elem) {
    validateEquals = !validator.isEmpty(elem.val(), { ignore_whitespace: true });

    return validateEquals;
}

function isSetDecimal(elem) {
    validateEquals = validator.isDecimal(elem.val());

    return validateEquals;
}

function ValidationAuthor(result) {
    $("#form0").submit(function () {
        var reultRes;

        var name = $("[name*='Name']");
        var lastName = $("[name*='LastName']");
        var fatherName = $("[name*='PatronymicName']");

        var arr = [name, lastName, fatherName];
        reultRes = SetFind(arr, isEmptyAndDecimal);

        var selectCountry = $("[name*='CountryBorn']");
        reultRes = SetFind([selectCountry], isSetCountry) && reultRes;

        var fatherName = $("[name*='DateBorn']");
        var info = $("[name*='Info']");

        reultRes = SetFind([fatherName, info], isSetEmpty) && reultRes;

        if (!reultRes) {
            return reultRes;
        }
        else {
            result();
        }
    });
}

function ValidationBook(result) {
    $("#form0").submit(function () {
        var reultRes;

        var number = $("[name*='Number']");
        var price = $("[name*='Price']");
        var discount = $("[name*='Discount']")

        reultRes = SetFind([number, price, discount], isSetDecimal);

        var name = $("#form0 input[name*='Name']");
        var description = $("[name*='Description']");
        var dateWrite = $("[name*='DateWrite']");
        var datePublication = $("[name*='DatePublication']");
        var author = $("[name*='AuthorId']");
        var publisher = $("[name*='PublisherId']");

        reultRes = SetFind(
            [name,
            description,
            dateWrite,
            datePublication,
            author,
                publisher], isSetEmpty) && reultRes;

        if (!reultRes) {
            return reultRes;
        }
        else {
            result();
        }
    });
}

function ValidationPublisher(result) {
    $("#form0").submit(function () {
        var name = $("#form0 input[name*='Name']");
        var info = $("[name*='Info']");

        reultRes = SetFind([name, info], isSetEmpty);

        if (!reultRes) {
            return reultRes;
        }
        else {
            result();
        }
    });
}