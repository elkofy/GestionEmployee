<script setup>
import useEmployees from '../composables/useEmployee';
import useAttendances from '../composables/useAttendances';
import useLeaveRequests from '../composables/useLeaveRequests';
import { ref, onBeforeMount, computed } from 'vue';
import { useRoute } from 'vue-router'
const route = useRoute();

const currentEmployee = ref(
    {
        firstName: '',
        lastName: '',
        email: '',
        phoneNumber: '',
        position: '',
        department: [],
        birthDate: ''
    }
);
const attendances = ref([]);
const leaveRequests = ref([]);
const errorMessage = ref('');
const isError = ref(false);

const newAttendance = ref({
    employeeId: route.params.id,
    startDate: '',
    endDate: ''
});

const newLeaveRequest = ref({
    employeeId: route.params.id,
    startDate: '',
    endDate: ''
});

const getEmployeeName = computed(() => {
    if (currentEmployee.value) {
        return `${currentEmployee.value.firstName} ${currentEmployee.value.lastName}`;
    }
    return 'Aucun employé';
})

const getBirthDate = computed(() => {
    if (currentEmployee.value) {
        let date = new Date(currentEmployee.value.birthDate);
        return `🎂 ${date.toDateString()}`;
    }
    return 'Aucune date de naissance';
})

const getFormattedDate = (date) => {
    let dateObj = new Date(date);
    return dateObj.toLocaleString();
}

const getDepartement = computed(() => {
    if (currentEmployee.value.departements) {
        let departements = currentEmployee.value.departements.join(', ')
        return `🏢 ${departements}`;
    }
    return '🏢 aucun département';
})
const displayError = (message) => {
    errorMessage.value = message;
    isError.value = true;
    setTimeout(() => {
        isError.value = false;
        errorMessage.value = '';
    }, 3000);
}

const submitAttendance = async () => {
    if (!newAttendance.value.startDate) {
        displayError('La date de début est obligatoire');
        return;
    } else if (!newAttendance.value.endDate) {
        displayError('La date de fin est obligatoire');
        return;
    }
    const res = await useAttendances('add-one', newAttendance.value);

    if (res.detail) {
        displayError(res.detail);
        return;
    } else {
        attendances.value = await useAttendances('fetch-one-by-employee-id', { id: route.params.id });
        newAttendance.value = {
            id: route.params.id,
            startDate: '',
            endDate: ''
        };
        my_modal_2.close();
    }
}

const submitLeaveRequest = async () => {
    if (!newLeaveRequest.value.startDate) {
        displayError('La date de début est obligatoire');
        return;
    } else if (!newLeaveRequest.value.endDate) {
        displayError('La date de fin est obligatoire');
        return;
    }
    const res = await useLeaveRequests('add-one', newLeaveRequest.value);

    if (res.detail) {
        displayError(res.detail);
        return;
    } else {
        attendances.value = await useLeaveRequests('fetch-one-by-employee-id', { id: route.params.id });
        newLeaveRequest.value = {
            id: route.params.id,
            startDate: '',
            endDate: ''
        };
        my_modal_3.close();
    }
}

onBeforeMount(async () => {
    const route = useRoute();
    currentEmployee.value = await useEmployees('fetch-one', { id: route.params.id });
    attendances.value = await useAttendances('fetch-one-by-employee-id', { id: route.params.id });
    leaveRequests.value = await useLeaveRequests('fetch-one-by-employee-id', { id: route.params.id });
})
</script>

<template>
    <h1 class="text-3xl font-bold text-center mb-10">{{ getEmployeeName }}</h1>
    <dialog id="my_modal_2" class="modal">
        <div class="modal-box h-2/3">
            <form method="dialog">
                <button class="btn btn-sm btn-circle btn-ghost absolute right-2 top-2">✕</button>
            </form>
            <div class="mb-3 w-11/12">
                <h3 class="font-bold text-lg">Ajouter une présence</h3>
                <div class="label">
                    <span class="label-text">Début</span>
                </div>
                <input type="datetime-local" v-model="newAttendance.startDate" placeholder="Date de début"
                    class="input input-bordered w-full max-w-xs" />
                <div class="label">
                    <span class="label-text">Fin </span>
                </div>
                <input v-model="newAttendance.endDate" type="datetime-local"  placeholder="Date de fin"
                    class="input input-bordered w-full max-w-xs" />
            </div>
            <button class="btn btn-primary btn-sm mb-3" @click="submitAttendance">Ajouter</button>
            <div v-if="isError" class="alert alert-error">
                <span>{{ errorMessage }}</span>
            </div>
        </div>

    </dialog>
    <dialog id="my_modal_3" class="modal">
        <div class="modal-box h-2/3">
            <form method="dialog">
                <button class="btn btn-sm btn-circle btn-ghost absolute right-2 top-2">✕</button>
            </form>
            <div class="mb-3 w-11/12">
                <h3 class="font-bold text-lg">Ajouter une présence</h3>
                <div class="label">
                    <span class="label-text">Date de début</span>
                </div> 
                <input type="datetime-local" v-model="newLeaveRequest.startDate" placeholder="Date de début"
                    class="input input-bordered w-full max-w-xs" />
                <div class="label">
                    <span class="label-text">Fin</span>
                </div>
                <input v-model="newLeaveRequest.endDate" type="datetime-local"  placeholder="Date de fin"
                    class="input input-bordered w-full max-w-xs" />
            </div>
            <button class="btn btn-primary btn-sm mb-3" @click="submitLeaveRequest">Ajouter</button>
            <div v-if="isError" class="alert alert-error">
                <span>{{ errorMessage }}</span> 
            </div>
        </div>

    </dialog>
    <div class="flex flex-row mx-5">
        <div class="card w-80 bg-base-300 shadow-xl">
            <div class="card-body items-start">
                <p>{{ getBirthDate }}</p>
                <p>📧 {{ currentEmployee.email }}</p>
                <p>📱 {{ currentEmployee.phoneNumber }}</p>
                <p>👨‍💻 {{ currentEmployee.position }}</p>
                <p>{{ getDepartement }}</p>
                <div class="card-actions justify-end">
                    <button class="btn btn-warning text-white btn-sm">Modifier</button>
                </div>
            </div>
        </div>
        <div class="flex flex-col w-11/12">
            <div class="flex flex-col justify-center items-center">
                <h3 class="text-2xl font-bold text-center mb-10">Feuille de temps</h3>
                <button class="btn btn-primary w-fit mb-5" onclick="my_modal_2.showModal()">Ajouter</button>
            </div>

            <div class="overflow-x-auto w-2/3 mx-auto mb-5">
                <table class="table">
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Date de début</th>
                            <th>Date de fin</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="attendance in attendances" :key="attendance.id" class="hover">
                            <td>{{ attendance.attendanceId }}</td>
                            <td>{{ getFormattedDate(attendance.startDate) }}</td>
                            <td>{{ getFormattedDate(attendance.endDate) }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="flex flex-col justify-center items-center">
                <h3 class="text-2xl font-bold text-center mb-5">Congés</h3>
                <button class="btn btn-primary"  onclick="my_modal_3.showModal()">Ajouter</button>
            </div>

            <div class="overflow-x-auto w-2/3 mx-auto">
                <table class="table">
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Date de la demande</th>
                            <th>Date de début</th>
                            <th>Date de fin</th>
                            <th>Statut</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="leaveRequest in leaveRequests" :key="leaveRequest.id" class="hover">
                            <td>{{ leaveRequest.leaveRequestId }}</td>
                            <td>{{ getFormattedDate(leaveRequest.requestDate) }}</td>
                            <td>{{ getFormattedDate(leaveRequest.startDate) }}</td>
                            <td>{{ getFormattedDate(leaveRequest.endDate) }}</td>
                            <td>{{ leaveRequest.leaveRequestStatusId }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</template>