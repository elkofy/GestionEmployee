const actions = {
    "fetch-one-by-employee-id": async (payload) => {
      return await fetch(`${import.meta.env.VITE_API_URL}/Attendances/employee/${payload.id}`, {
        method: "GET",
      }).then((response) => response.json());
    },
    "add-one": async (payload) => {
      return await fetch(`${import.meta.env.VITE_API_URL}/Attendances`, {
        method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
        body: JSON.stringify(payload),
      }).then((response) => response.json());
    },
    // "update-one": async (payload) => {
    //   return await fetch(`${import.meta.env.VITE_API_URL}/Departments/${payload.id}`, {
    //     method: "PUT",
    //       headers: {
    //         "Content-Type": "application/json",
    //       },
    //     body: JSON.stringify(payload),
    //   }).then((response) => response.json());
    // },
    // "delete-one": async (payload) => {
    //   return await fetch(`${import.meta.env.VITE_API_URL}/Departments/${payload.id}`, {
    //     method: "DELETE",
    //   }).then((response) => response.json());
    // }
  };
  
  export default async function useAttendances(action, payload) {
    return await actions[action](payload);
  }
  