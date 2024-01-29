<script setup>
import useEmployees from '../composables/useEmployee';
import { ref, onBeforeMount, computed } from 'vue';

const employees = ref([]);
onBeforeMount(async () => {
    employees.value = await useEmployees();
})
</script>

<template>
    <h1 class="text-3xl font-bold text-center mb-10">Employées</h1>
    <div class="overflow-x-auto w-2/3 mx-auto">
        <table class="table">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Prénom</th>
                    <th>Nom</th>
                    <th>Email</th>
                    <th class="text-center">Actions</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="employee in employees" :key="employee.id" class="hover">
                    <td v-for="key in employee">{{ key }}</td>
                    <td class="flex flex-col w-32 gap-2 justify-center mx-auto">
                        <a class="btn btn-primary btn-xs"><router-link :to="`/employees/${employee.id}`"> Voir</router-link> </a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>