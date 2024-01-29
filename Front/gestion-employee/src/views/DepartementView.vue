<template>
    <div class="flex flex-col w-2/3 justify-center items-center  mx-auto">
        <h1 class="mx-auto text-3xl font-bold">Departement</h1>
        <button class="btn btn-primary btn-s w-fit" onclick="my_modal_2.showModal()"
            @click="isModify = false">Ajouter</button>
    </div>
    <dialog id="my_modal_2" class="modal">
        <div class="modal-box  h-2/3">
            <form method="dialog">
                <button class="btn btn-sm btn-circle btn-ghost absolute right-2 top-2">✕</button>
            </form>
            <div class="mb-3 w-11/12">
                <h3 class="font-bold text-lg">{{ getAction }} un Departement</h3>
                <div class="label">
                    <span class="label-text">Nom </span>
                </div>
                <input v-model="newDepartement.name" type="text" placeholder="Nom"
                    class="input input-bordered w-full max-w-xs" />
                <div class="label">
                    <span class="label-text">Adresse </span>
                </div>
                <input v-model="newDepartement.address" type="text" placeholder="Adresse"
                    class="input input-bordered w-full max-w-xs" />
                <div class="label">
                    <span class="label-text">Description </span>
                </div>
                <input v-model="newDepartement.description" type="text" placeholder="Description"
                    class="input input-bordered w-full max-w-xs" />
            </div>

            <button class="btn btn-primary btn-sm mb-3" @click="submitDepartement">{{ getAction }}</button>
            <div v-if="isError" class="alert alert-error">
                <span>{{ errorMessage }}</span>
            </div>
        </div>

    </dialog>
    <div class="overflow-x-auto w-2/3 mx-auto">
        <table class="table">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Nom</th>
                    <th>Adresse</th>
                    <th>Description</th>
                    <th class="text-center">Actions</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="departement in departements" :key="departement.id" class="hover">
                    <td v-for="key in departement">{{ key }}</td>

                    <td class="flex flex-col w-32 gap-2 justify-center mx-auto">
                        <button class="btn btn-warning btn-xs" onclick="my_modal_2.showModal()"
                            @click="modifyRow(departement)">Modifier</button>
                        <button class="btn btn-error btn-xs" @click="deleteRow(departement.id)">Supprimer</button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>
<script setup>
import useDepartements from '../composables/useDepartements';
import { ref, onBeforeMount, computed } from 'vue';

const departements = ref([]);
const newDepartement = ref({
    name: '',
    address: '',
    description: ''
});

const getAction = computed(() => {
    return isModify.value ? 'Modifier' : 'Ajouter';
})
const errorMessage = ref('');
const isError = ref(false);
const isModify = ref(false);
const displayError = (message) => {
    errorMessage.value = message;
    isError.value = true;
    setTimeout(() => {
        isError.value = false;
        errorMessage.value = '';
    }, 3000);
}
const deleteRow = async (id) => {
    const res = await useDepartements('delete-one', { id: id });
      departements.value = await useDepartements();

}
const submitDepartement = async () => {
    if (!newDepartement.value.name) {
        displayError('Le nom est obligatoire');
        return;
    } else if (!newDepartement.value.address) {
        displayError('L\'adresse est obligatoire');
        return;
    } else if (!newDepartement.value.description) {
        displayError('La description est obligatoire');
        return;
    }
    const res = isModify.value ? await useDepartements('update-one', newDepartement.value) : await useDepartements('add-one', newDepartement.value);

    if (res.detail) {
        displayError(res.detail);
        return;
    } else {
        departements.value = await useDepartements();
        newDepartement.value = {
            name: '',
            address: '',
            description: ''
        };
        my_modal_2.close();
    }
}

const modifyRow = (departement) => {
    isModify.value = true;
    newDepartement.value = departement;
}
onBeforeMount(async () => {
    departements.value = await useDepartements();
});

</script>