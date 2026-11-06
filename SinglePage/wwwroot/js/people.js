const form = document.querySelector('#person-form');
const idInput = document.querySelector('#person-id');
const firstNameInput = document.querySelector('#first-name');
const lastNameInput = document.querySelector('#last-name');
const submitButton = document.querySelector('#submit-button');
const cancelButton = document.querySelector('#cancel-edit');
const list = document.querySelector('#people-list');
const emptyState = document.querySelector('#empty-state');
const errorBox = document.querySelector('#form-error');
const notice = document.querySelector('#notice');

async function request(url, options) {
  const response = await fetch(url, {
    ...options,
    headers: { 'Content-Type': 'application/json', ...options?.headers }
  });
  if (!response.ok) {
    let message = `Request failed (${response.status})`;
    try {
      const data = await response.json();
      message = data.message || data.title || message;
    } catch { /* The response may not contain JSON. */ }
    throw new Error(message);
  }
  return response.status === 204 ? null : response.json();
}

function showNotice(message) {
  notice.textContent = message;
  notice.hidden = false;
  window.setTimeout(() => { notice.hidden = true; }, 3000);
}

async function loadPeople() {
  list.replaceChildren();
  try {
    const people = await request('/api/people');
    emptyState.hidden = people.length > 0;
    for (const person of people) {
      const row = document.createElement('tr');
      for (const value of [person.id, person.firstName, person.lastName]) {
        const cell = document.createElement('td');
        cell.textContent = value;
        row.append(cell);
      }

      const actions = document.createElement('td');
      const edit = document.createElement('button');
      edit.type = 'button';
      edit.className = 'btn btn-sm btn-outline-primary me-2';
      edit.textContent = 'Edit';
      edit.addEventListener('click', () => startEdit(person));
      const remove = document.createElement('button');
      remove.type = 'button';
      remove.className = 'btn btn-sm btn-outline-danger';
      remove.textContent = 'Delete';
      remove.addEventListener('click', () => deletePerson(person));
      actions.append(edit, remove);
      row.append(actions);
      list.append(row);
    }
  } catch (error) {
    emptyState.textContent = error.message;
    emptyState.hidden = false;
  }
}

function startEdit(person) {
  idInput.value = person.id;
  firstNameInput.value = person.firstName;
  lastNameInput.value = person.lastName;
  submitButton.textContent = 'Save changes';
  cancelButton.hidden = false;
}

function resetForm() {
  form.reset();
  idInput.value = '';
  submitButton.textContent = 'Add person';
  cancelButton.hidden = true;
  errorBox.hidden = true;
}

form.addEventListener('submit', async (event) => {
  event.preventDefault();
  errorBox.hidden = true;
  const firstName = firstNameInput.value.trim();
  const lastName = lastNameInput.value.trim();
  if (!firstName || !lastName) {
    errorBox.textContent = 'Enter both names.';
    errorBox.hidden = false;
    return;
  }

  const id = Number(idInput.value);
  const editing = Boolean(id);
  submitButton.disabled = true;
  try {
    await request(editing ? `/api/people/${id}` : '/api/people', {
      method: editing ? 'PUT' : 'POST',
      body: JSON.stringify(editing ? { id, firstName, lastName } : { firstName, lastName })
    });
    resetForm();
    showNotice(editing ? 'Person updated.' : 'Person added.');
    await loadPeople();
  } catch (error) {
    errorBox.textContent = error.message;
    errorBox.hidden = false;
  } finally {
    submitButton.disabled = false;
  }
});

async function deletePerson(person) {
  if (!window.confirm(`Delete ${person.firstName} ${person.lastName}?`)) return;
  try {
    await request(`/api/people/${person.id}`, { method: 'DELETE' });
    if (Number(idInput.value) === person.id) resetForm();
    showNotice('Person deleted.');
    await loadPeople();
  } catch (error) {
    showNotice(error.message);
  }
}

cancelButton.addEventListener('click', resetForm);
loadPeople();
