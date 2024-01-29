<template>
  <div class="navbar bg-base-100">
    <div class="flex-1">
      
      <a class="btn btn-ghost text-xl">
        <router-link to="/leave-requests">SGE</router-link></a>
    </div>
    <div class="flex-none">
      <ul class="menu menu-horizontal px-1">
        <li> <router-link to="/calendar">Calendrier</router-link></li>
        <li>
          <details>
            <summary>
              <span>Paramètres</span> 
            </summary>
            <ul class="p-2 bg-base-100 rounded-t-none">
              <li> <router-link to="/employees">Employés</router-link></li>
              <li> <router-link to="/departements">Departements</router-link></li>
            </ul>
          </details>
        </li>
      </ul>
    </div>
  </div>

  <router-view></router-view>
</template>