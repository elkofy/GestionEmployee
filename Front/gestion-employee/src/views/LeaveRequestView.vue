<script setup>
import LeaveRequestCard from "../components/LeaveRequestCard.vue";
</script>

<template>
    <h1 class="text-3xl font-bold text-center mb-10">Demande de congés</h1>
    <div class="flex flex-row mx-10 gap-10">
        <div class="w-1/6">
            <p class="text-xl font-bold">Statut :</p>
            <div class="form-control">
                <label class="label cursor-pointer">
                    <span class="label-text">En attente</span>
                    <input type="radio" name="radio-10" class="radio checked:bg-red-500" />
                </label>
            </div>
            <div class="form-control">
                <label class="label cursor-pointer">
                    <span class="label-text">Validée</span>
                    <input type="radio" name="radio-10" class="radio checked:bg-blue-500" checked />
                </label>
            </div>
            <div class="form-control">
                <label class="label cursor-pointer">
                    <span class="label-text">Tous</span>
                    <input type="radio" name="radio-10" class="radio checked:bg-red-500" checked />
                </label>
            </div>


        </div>
        <div class="w-3/4 grid grid-cols-3">
            <LeaveRequestCard />
            <LeaveRequestCard />
            <LeaveRequestCard />
        </div>
    </div>
</template>