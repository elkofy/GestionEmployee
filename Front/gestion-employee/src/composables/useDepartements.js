const actions = {
  "fetch-all": async () => {
    return await fetch(`${import.meta.env.VITE_API_URL}/Departments`, {
      method: "GET",
    }).then((response) => response.json());
  },
  "add-one": async (payload) => {
    return await fetch(`${import.meta.env.VITE_API_URL}/Departments`, {
      method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
      body: JSON.stringify(payload),
    }).then((response) => response.json());
  },
  "update-one": async (payload) => {
    return await fetch(`${import.meta.env.VITE_API_URL}/Departments/${payload.id}`, {
      method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
      body: JSON.stringify(payload),
    }).then((response) => response.json());
  },
  "delete-one": async (payload) => {
    return await fetch(`${import.meta.env.VITE_API_URL}/Departments/${payload.id}`, {
      method: "DELETE",
    }).then((response) => response.json());
  }
};

export default async function useDepartements(action = 'fetch-all', payload) {
  return await actions[action](payload);
}
