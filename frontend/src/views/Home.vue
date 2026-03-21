<template>
    <div class="min-h-screen bg-despacho-oscuro text-despacho-fondo font-sans selection:bg-despacho-rojo selection:text-white relative">
      
      <nav class="border-b border-white/10 bg-despacho-oscuro/80 backdrop-blur-md sticky top-0 z-50">
        <div class="max-w-7xl mx-auto px-6 md:px-10 py-4 flex justify-between items-center">
          <div class="flex items-center gap-3">
            <div class="w-8 h-8 bg-despacho-rojo rounded-sm flex items-center justify-center font-bold text-white shadow-[0_0_10px_rgba(128,0,0,0.5)]">
              DJ
            </div>
            <span class="text-xl font-bold tracking-widest uppercase hidden md:inline-block">Despacho Jurídico</span>
          </div>
          
          <div class="flex items-center gap-4">
            <template v-if="!isLogged">
              <router-link 
                to="/login" 
                class="text-xs font-bold uppercase tracking-widest text-despacho-tierra hover:text-white transition-colors"
              >
                Iniciar Sesión
              </router-link>
              <router-link 
                to="/registro" 
                class="text-xs font-bold uppercase tracking-widest bg-despacho-rojo text-white px-5 py-2.5 rounded-xl hover:bg-despacho-rojo/90 transition-all shadow-lg active:scale-95"
              >
                Crear Cuenta
              </router-link>
            </template>

            <template v-else>
              <div class="flex items-center gap-3">
                
                <template v-if="userRole === 'Abogado' || userRole === 'Administrador'">
                  <router-link 
                    to="/dashboard" 
                    class="text-[10px] font-bold uppercase tracking-widest bg-white/5 text-despacho-tierra px-4 py-2 rounded-lg hover:bg-white/10 transition-all border border-white/5 hidden md:block"
                  >
                    Dashboard
                  </router-link>
                  <router-link 
                    to="/citas" 
                    class="text-[10px] font-bold uppercase tracking-widest bg-white/5 text-despacho-tierra px-4 py-2 rounded-lg hover:bg-white/10 transition-all border border-white/5 hidden md:block"
                  >
                    Ver Agenda
                  </router-link>
                  <router-link 
                    to="/casos" 
                    class="text-[10px] font-bold uppercase tracking-widest bg-white/5 text-despacho-tierra px-4 py-2 rounded-lg hover:bg-white/10 transition-all border border-white/5"
                  >
                    Gestionar Casos
                  </router-link>
                </template>

                <template v-if="userRole === 'Cliente' || userRole === 'Administrador'">
                  <router-link 
                    to="/mis-casos" 
                    class="text-[10px] font-bold uppercase tracking-widest bg-despacho-rojo text-white px-4 py-2 rounded-lg hover:bg-despacho-rojo/90 transition-all shadow-lg active:scale-95"
                  >
                    Mis Expedientes
                  </router-link>
                </template>

                <div class="h-8 w-[1px] bg-white/10 mx-2 hidden md:block"></div>

                <div class="hidden sm:flex flex-col items-end mr-2">
                  <span class="text-[10px] font-bold text-despacho-tierra uppercase tracking-tighter">Bienvenido</span>
                  <span class="text-xs font-medium text-white truncate max-w-[100px]">{{ userName }}</span>
                </div>

                <button 
                  @click="cerrarSesion" 
                  class="p-2.5 bg-white/5 text-despacho-rojo hover:bg-despacho-rojo hover:text-white rounded-xl transition-all active:scale-95 border border-white/5 group"
                  title="Cerrar Sesión"
                >
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-5 h-5">
                    <path stroke-linecap="round" stroke-linejoin="round" d="M15.75 9V5.25A2.25 2.25 0 0013.5 3h-6a2.25 2.25 0 00-2.25 2.25v13.5A2.25 2.25 0 007.5 21h6a2.25 2.25 0 002.25-2.25V15m3 0l3-3m0 0l-3-3m3 3H9" />
                  </svg>
                </button>
              </div>
            </template>
          </div>
        </div>
      </nav>
  
      <main class="max-w-7xl mx-auto px-6 md:px-10 pt-20 pb-24 md:pt-32 flex flex-col items-center text-center relative">
        <div class="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[600px] h-[600px] bg-despacho-rojo/5 rounded-full blur-3xl -z-10 pointer-events-none"></div>
  
        <span class="text-despacho-rosa font-bold tracking-[0.2em] text-sm md:text-base mb-6 uppercase">
          Experiencia - Firmeza - Resultados
        </span>
        
        <h1 class="md:text-7xl text-4xl font-black mb-8 leading-tight max-w-4xl text-transparent bg-clip-text uppercase bg-gradient-to-br from-white to-despacho-tierra">
          Protegemos tu patrimonio y tranquilidad legal.
        </h1>
        
        <p class="text-lg md:text-xl text-despacho-fondo/70 max-w-2xl mb-12 leading-relaxed">
          Brindamos representación legal estratégica y personalizada. No dejes tus problemas legales para mañana, obtén claridad hoy mismo.
        </p>
  
        <button 
          @click="abrirModalCita"
          class="group relative inline-flex items-center justify-center px-8 py-4 text-base font-bold text-white transition-all duration-200 bg-despacho-rojo border border-transparent rounded-full shadow-[0_10px_30px_rgba(128,0,0,0.4)] hover:shadow-[0_15px_40px_rgba(128,0,0,0.6)] hover:-translate-y-1 active:scale-95 uppercase tracking-widest"
        >
          <span>Agendar Asesoría Ahora</span>
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-5 h-5 ml-3 group-hover:translate-x-1 transition-transform">
            <path stroke-linecap="round" stroke-linejoin="round" d="M13.5 4.5L21 12m0 0l-7.5 7.5M21 12H3" />
          </svg>
        </button>
      </main>
  
      <section class="border-t border-white/5 bg-black/20 py-24">
        <div class="max-w-7xl mx-auto px-6 md:px-10">
          <div class="text-center mb-16">
            <h2 class="text-3xl font-bold mb-4 text-white">Áreas de Práctica</h2>
            <div class="h-1 w-16 bg-despacho-rojo mx-auto rounded-full"></div>
          </div>
  
          <div class="grid grid-cols-1 md:grid-cols-3 gap-8">
            <div class="bg-white/5 backdrop-blur-sm p-8 rounded-3xl border border-white/10 hover:border-despacho-rojo/30 transition-all hover:-translate-y-2">
              <div class="w-12 h-12 bg-despacho-rojo/20 rounded-xl flex items-center justify-center mb-6 text-despacho-rojo">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-6 h-6"><path stroke-linecap="round" stroke-linejoin="round" d="M18 18.72a9.094 9.094 0 003.741-.479 3 3 0 00-4.682-2.72m.94 3.198l.001.031c0 .225-.012.447-.037.666A11.944 11.944 0 0112 21c-2.17 0-4.207-.576-5.963-1.584A6.062 6.062 0 016 18.719m12 0a5.971 5.971 0 00-.941-3.197m0 0A5.995 5.995 0 0012 12.75a5.995 5.995 0 00-5.058 2.772m0 0a3 3 0 00-4.681 2.72 8.986 8.986 0 003.74.477m.94-3.197a5.971 5.971 0 00-.94 3.197M15 6.75a3 3 0 11-6 0 3 3 0 016 0zm6 3a2.25 2.25 0 11-4.5 0 2.25 2.25 0 014.5 0zm-13.5 0a2.25 2.25 0 11-4.5 0 2.25 2.25 0 014.5 0z" /></svg>
              </div>
              <h3 class="text-xl font-bold mb-3 text-white">Derecho Familiar</h3>
              <p class="text-sm text-despacho-fondo/60 leading-relaxed">Divorcios, pensiones alimenticias, custodia y adopciones. Tratamos tus asuntos de familia con la máxima sensibilidad y firmeza.</p>
            </div>
  
            <div class="bg-white/5 backdrop-blur-sm p-8 rounded-3xl border border-white/10 hover:border-despacho-rojo/30 transition-all hover:-translate-y-2">
              <div class="w-12 h-12 bg-despacho-rojo/20 rounded-xl flex items-center justify-center mb-6 text-despacho-rojo">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-6 h-6"><path stroke-linecap="round" stroke-linejoin="round" d="M20.25 14.15v4.25c0 1.094-.787 2.036-1.872 2.18-2.087.277-4.216.42-6.378.42s-4.291-.143-6.378-.42c-1.085-.144-1.872-1.086-1.872-2.18v-4.25m16.5 0a2.18 2.18 0 00.75-1.661V8.706c0-1.081-.768-2.015-1.837-2.175a48.114 48.114 0 00-3.413-.387m4.5 8.006c-.194.165-.42.295-.673.38A23.978 23.978 0 0112 15.75c-2.648 0-5.195-.429-7.577-1.22a2.016 2.016 0 01-.673-.38m0 0A2.18 2.18 0 013 12.489V8.706c0-1.081.768-2.015 1.837-2.175a48.111 48.111 0 013.413-.387m7.5 0V5.25A2.25 2.25 0 0013.5 3h-3a2.25 2.25 0 00-2.25 2.25v.894m7.5 0a48.667 48.667 0 00-7.5 0M12 12.75h.008v.008H12v-.008z" /></svg>
              </div>
              <h3 class="text-xl font-bold mb-3 text-white">Derecho Laboral</h3>
              <p class="text-sm text-despacho-fondo/60 leading-relaxed">Defensa ante despidos injustificados, cálculo de finiquitos y representación ante juntas de conciliación para trabajadores y empresas.</p>
            </div>
  
            <div class="bg-white/5 backdrop-blur-sm p-8 rounded-3xl border border-white/10 hover:border-despacho-rojo/30 transition-all hover:-translate-y-2">
              <div class="w-12 h-12 bg-despacho-rojo/20 rounded-xl flex items-center justify-center mb-6 text-despacho-rojo">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-6 h-6"><path stroke-linecap="round" stroke-linejoin="round" d="M12 21v-8.25M15.75 21v-8.25M8.25 21v-8.25M3 9l9-6 9 6m-1.5 12V10.332A48.36 48.36 0 0012 9.75c-2.551 0-5.056.2-7.5.582V21M3 21h18M12 6.75h.008v.008H12V6.75z" /></svg>
              </div>
              <h3 class="text-xl font-bold mb-3 text-white">Derecho Civil</h3>
              <p class="text-sm text-despacho-fondo/60 leading-relaxed">Contratos, arrendamientos, juicios hipotecarios y recuperación de cartera vencida. Protegemos tus bienes e intereses comerciales.</p>
            </div>
          </div>
        </div>
      </section>
  
      <footer class="text-center py-8 border-t border-white/5 text-despacho-fondo/40 text-xs tracking-widest uppercase">
        © 2026 Despacho Jurídico. Todos los derechos reservados.
      </footer>
  
      <ModalCita 
        v-if="mostrarModalCita" 
        ref="refModalCita"
        @cerrar="mostrarModalCita = false"
        @agendar="procesarAgendarCita"
      />
  
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue'
  import { useRouter } from 'vue-router'
  import axios from 'axios'
  import ModalCita from '../components/ModalCita.vue'
  import Swal from 'sweetalert2'
  
  const router = useRouter()
  const mostrarModalCita = ref(false)
  const refModalCita = ref(null)
  
  const isLogged = ref(false)
  const userName = ref('')
  const userRole = ref('')
  
  const decodificarJWT = (token) => {
    try {
      const base64Url = token.split('.')[1]
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/')
      const jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function(c) {
          return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)
      }).join(''))
      return JSON.parse(jsonPayload)
    } catch (error) {
      return null
    }
  }
  
  onMounted(() => {
    const token = localStorage.getItem('token')
    if (token) {
      const payload = decodificarJWT(token)
      if (payload) {
        isLogged.value = true
        userName.value = payload.Nombre
        // Clave del Claim de Rol en .NET
        userRole.value = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
      }
    }
  })
  
  const cerrarSesion = () => {
    localStorage.removeItem('token') 
    isLogged.value = false 
    userName.value = ''
    userRole.value = ''
  }
  
  const abrirModalCita = () => {
    if (!isLogged.value) {
      router.push('/login')
    } else {
      mostrarModalCita.value = true
    }
  }
  
  const procesarAgendarCita = async (datosCita) => {
    const token = localStorage.getItem('token')
    const payload = decodificarJWT(token)
    const userId = payload ? payload.sub : null
  
    if (!userId) {
      refModalCita.value?.setMensajeError("Error de sesión. Vuelve a iniciar sesión.")
      return
    }
  
    const payloadBackend = {
      id_user: parseInt(userId),
      FechaHora: datosCita.FechaHora,
      Descripcion: datosCita.Descripcion
    }
  
    try {
      await axios.post('http://localhost:5039/api/citas', payloadBackend, {
        headers: { Authorization: `Bearer ${token}` }
      })
      
      mostrarModalCita.value = false
      
      Swal.fire({
        icon: 'success',
        title: '¡Asesoría Agendada!',
        text: 'Tu cita ha sido registrada con éxito. Te esperamos.',
        background: '#1a1a1c',
        color: '#fdfbd4',
        confirmButtonColor: '#8B0000',
        confirmButtonText: 'Entendido'
      })
      
    } catch (error) {
      const mensajeDelBackend = error.response?.data || "Ocurrió un error al agendar."
      refModalCita.value?.setMensajeError(typeof mensajeDelBackend === 'string' ? mensajeDelBackend : "Error desconocido.")
    }
  }
  </script>