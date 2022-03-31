const AccountSelect = document.getElementById("Accounts");
const Balance = document.getElementById("balance");
const Transaction = document.getElementById("transactions");

const getTransactions = async (accountId) => {
    const response = await fetch(window.location.href + "Home/GetTransactions", {
        method: 'POST',
        body: accountId,
        headers: {
            'Content-Type': 'application/json'
        }
    });

    const json = await response.json();
    let amount = 0.00;
    Transaction.innerHTML = "";
    json.forEach(t => {
        amount += t.total;
        insertTransaction(t.id, t.description, t.total)
    });

    Balance.innerHTML = amount;
}

AccountSelect.multiple = false;

getTransactions(parseInt(AccountSelect.value));

AccountSelect.onchange = async (e) => {
    const accountId = e.path[0].value;
    getTransactions(accountId)
}




const insertTransaction = (id, name, amount) => {
    if (amount < 0) {
        Transaction.innerHTML += `
        <tr class="table-danger">
            <td>${name}</td>
            <td class="fw-bold">${amount}</th>
        </tr>
        `;
    } else {
        Transaction.innerHTML += `
        <tr class="table-success">
            <td>${name}</td>
            <td class="fw-bold">${amount}</th>
        </tr>
        `;
    }
}

