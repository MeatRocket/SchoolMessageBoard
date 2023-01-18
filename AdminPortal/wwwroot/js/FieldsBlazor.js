var prevId = "1";

function Fields() {
    document.querySelector("#Field").addEventListener("change", function (event) {
        var id = event.target.value;

        if (id == "") {
            document.querySelectorAll("._checkBox").forEach(checkbox => checkbox.classList.add("_hide"))
            return;
        }

        document.querySelector('[id=' + '"' + id + '"]').classList.remove("_hide");

        if (prevId != id) {
            var previosInput = document.querySelector('[id=' + '"' + prevId + '"]');

            previosInput.classList.add("_hide");
            previosInput.querySelectorAll("input").forEach(x => x.checked = false);
            prevId = id;
        }
    })
}