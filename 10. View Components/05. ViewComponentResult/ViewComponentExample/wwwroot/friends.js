
document
    .querySelector("#load-friends-button")
    .addEventListener("click", async () => {
        let response = await fetch("friend-list", { method: "GET" })
        let content = await response.text()

        document
            .querySelector("#list")
            .innerHTML = content
    })