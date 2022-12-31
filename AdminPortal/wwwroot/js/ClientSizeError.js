    document.querySelector("._uploadFile").addEventListener("change", function (event) {

        var Files = document.querySelector("._uploadFile").files;
        var totalSize = 0;
        for (var i = 0; i < Files.length; i++) {
            totalSize += Files[i].size;
        }
        if (totalSize > 500000000) {
            var listItem = document.createElement("li");
            listItem.innerHTML = "Files size should not exceed 500MB!";
            listItem.classList.add("_clientError");
            document.querySelector("._errorList").appendChild(listItem);
        }
        else {
            if (document.querySelector("._clientError") != null)
                document.querySelector("._clientError").remove();
        }
    })