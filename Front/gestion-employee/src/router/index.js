import * as VueRouter from 'vue-router'

const router = VueRouter.createRouter({
    history: VueRouter.createWebHistory(),
    routes: [
        {
            path: '/',
            name: 'LeaveRequest',
            component: () => import('../views/LeaveRequestView.vue')
        },
        {
            path: '/departements',
            name: 'Departement',
            component: () => import('../views/DepartementView.vue')
        },
        {
            path: '/leave-requests',
            name: 'LeaveRequest',
            component: () => import('../views/LeaveRequestView.vue')
        },
        {
            path: '/calendar',
            name: 'Calendar',
            component: () => import('../views/CalendarView.vue')
        }, {
            path: '/employees',
            name: 'Employee',
            component: () => import('../views/EmployeeListView.vue'),
          
            
    
        },
        {
            path: '/employees/:id',
            name: 'EmployeeDetails',
            component: () => import('../views/EmployeeView.vue')
        }
    ]
})

export default router;