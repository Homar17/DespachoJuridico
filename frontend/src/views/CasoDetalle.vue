<template>
    <div class="min-h-screen bg-despacho-oscuro p-6 md:p-10 text-despacho-fondo relative font-sans">
      <div class="max-w-5xl mx-auto">
        
        <header class="mb-10">
          <button 
            v-if="!editando"
            @click="volver" 
            class="flex items-center gap-2 text-despacho-tierra hover:text-white transition-colors text-sm font-bold uppercase tracking-widest mb-6 group"
          >
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2.5" stroke="currentColor" class="w-5 h-5 transform group-hover:-translate-x-1 transition-transform">
              <path stroke-linecap="round" stroke-linejoin="round" d="M10.5 19.5L3 12m0 0l7.5-7.5M3 12h18" />
            </svg>
            {{ esCliente ? 'Volver a Mis Expedientes' : 'Volver a Gestión' }}
          </button>
  
          <div v-if="!cargando && caso" class="flex flex-col md:flex-row md:items-start justify-between gap-6">
            <div class="flex-1">
              <div class="flex items-center gap-4 mb-2">
                <h2 v-if="!editando" class="text-3xl md:text-4xl font-black tracking-tight text-white">{{ caso.titulo }}</h2>
                <input 
                  v-else
                  v-model="formularioEdicion.titulo"
                  type="text"
                  class="flex-1 text-3xl md:text-4xl font-black tracking-tight text-white bg-white/5 border border-white/10 rounded-xl px-4 py-1 focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all"
                />
  
                <span v-if="!editando" class="bg-despacho-rojo/20 text-despacho-rosa text-xs font-black px-4 py-1.5 rounded-full uppercase tracking-tighter border border-despacho-rosa/50 whitespace-nowrap">
                  {{ caso.estatus?.descripcion || 'Pendiente' }}
                </span>
                <div v-else class="relative w-40">
                  <select 
                    v-model.number="formularioEdicion.id_estatus"
                    class="w-full text-xs font-black px-4 py-2 rounded-full uppercase tracking-tighter bg-white/10 border border-despacho-rosa/50 text-despacho-rosa appearance-none cursor-pointer focus:outline-none focus:ring-1 focus:ring-despacho-rojo"
                  >
                    <option v-for="est in listaEstatus" :key="est.id" :value="est.id" class="bg-despacho-oscuro text-white">{{ est.descripcion }}</option>
                  </select>
                </div>
              </div>
              <div class="h-1 w-20 bg-despacho-rojo rounded-full mt-2 shadow-[0_0_15px_rgba(128,0,0,0.6)]"></div>
            </div>
  
            <div v-if="!esCliente" class="flex gap-3">
              <button 
                v-if="!editando"
                @click="activarEdicion"
                class="px-5 py-2.5 bg-white/5 border border-white/10 text-despacho-fondo font-semibold rounded-xl hover:bg-white/10 transition-all active:scale-95 flex items-center gap-2 group"
              >
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-4 h-4 text-despacho-tierra group-hover:text-white"><path stroke-linecap="round" stroke-linejoin="round" d="M16.862 4.487l1.687-1.688a1.875 1.875 0 112.652 2.652L10.582 16.07a4.5 4.5 0 01-1.897 1.13L6 18l.8-2.685a4.5 4.5 0 011.13-1.897l8.932-8.931zm0 0L19.5 7.125M18 14v4.75A2.25 2.25 0 0115.75 21H5.25A2.25 2.25 0 013 18.75V8.25A2.25 2.25 0 015.25 6H10" /></svg>
                Editar
              </button>
  
              <button 
                v-if="editando"
                @click="cancelarEdicion"
                class="px-5 py-2.5 border border-white/10 text-despacho-fondo/70 font-semibold rounded-xl hover:bg-white/5 hover:text-white transition-all active:scale-95"
              >
                Cancelar
              </button>
              <button 
                v-if="editando"
                @click="guardarCambios"
                :disabled="guardando"
                class="px-5 py-2.5 bg-despacho-rojo text-white font-bold rounded-xl shadow-lg hover:bg-despacho-rojo/90 transition-all active:scale-95 disabled:opacity-50"
              >
                {{ guardando ? 'Guardando...' : 'Confirmar Cambios' }}
              </button>
            </div>
          </div>
        </header>
  
        <div v-if="cargando" class="flex flex-col items-center justify-center py-32 text-despacho-fondo/20">
          <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-despacho-rojo mb-4"></div>
          <p class="font-medium tracking-widest uppercase text-xs">Abriendo expediente...</p>
        </div>
  
        <main v-else-if="caso" class="grid grid-cols-1 md:grid-cols-3 gap-8">
          
          <div class="md:col-span-2 space-y-8">
            
            <section class="bg-white/5 backdrop-blur-sm p-8 rounded-3xl border border-white/10 shadow-xl relative overflow-hidden">
              <div class="absolute top-0 left-10 w-32 h-32 bg-despacho-rojo/5 rounded-full blur-3xl -z-10"></div>
              <h3 class="text-[10px] font-bold uppercase tracking-[0.2em] text-despacho-tierra mb-6">Detalles del Asunto</h3>
              
              <p v-if="!editando" class="text-despacho-fondo/90 leading-relaxed whitespace-pre-wrap text-lg italic serif">
                "{{ caso.descripcion || 'No hay descripción disponible.' }}"
              </p>
              <textarea 
                v-else
                v-model="formularioEdicion.descripcion"
                rows="10"
                class="w-full bg-black/20 border border-white/10 text-despacho-fondo rounded-xl px-4 py-3 focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all resize-none text-lg leading-relaxed"
              ></textarea>
            </section>
  
            <section class="bg-black/20 p-8 rounded-3xl border border-dashed border-white/10">
               <div class="flex items-center gap-3 mb-8">
                  <div class="p-2 bg-despacho-rojo/10 rounded-lg text-despacho-rojo">
                     <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-5 h-5"><path stroke-linecap="round" stroke-linejoin="round" d="M12 6v6h4.5m4.5 0a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
                  </div>
                  <h3 class="font-bold text-white uppercase tracking-widest text-sm">Historial de Actuaciones</h3>
               </div>
               
               <div v-if="historial.length === 0" class="text-center text-despacho-fondo/40 text-sm py-10 italic">
                 Aún no hay movimientos registrados.
               </div>
  
               <div v-else class="space-y-0">
                 <div v-for="item in historial" :key="item.id" class="flex gap-4 group">
                    <div class="font-mono text-despacho-tierra text-[10px] w-20 pt-1 text-right">
                      {{ formatearFechaHistorial(item.fechaHora) }}
                    </div>
                    <div class="flex-1 pb-8 border-l border-white/10 pl-6 relative">
                       <div class="absolute -left-1.5 top-1.5 w-3 h-3 bg-despacho-oscuro border-2 border-despacho-rojo rounded-full shadow-[0_0_8px_rgba(128,0,0,0.8)]"></div>
                       <p class="font-bold text-despacho-fondo text-sm">{{ item.descripcion }}</p>
                       <p class="text-[10px] text-despacho-fondo/30 mt-1 uppercase">{{ new Date(item.fechaHora).toLocaleTimeString('es-MX', { hour: '2-digit', minute: '2-digit' }) }} HRS</p>
                    </div>
                 </div>
               </div>
            </section>
          </div>
  
          <aside class="space-y-6">
            <div class="bg-white/5 p-6 rounded-3xl border border-white/10 shadow-xl relative overflow-hidden">
              <h3 class="text-[10px] font-bold uppercase tracking-widest text-despacho-tierra mb-4">Información del Cliente</h3>
              <div class="flex items-center gap-4">
                <div class="w-12 h-12 rounded-full bg-despacho-rojo/20 border border-despacho-rojo/40 flex items-center justify-center text-white font-black">
                  {{ caso.usuario?.nombre ? caso.usuario.nombre.charAt(0).toUpperCase() : '?' }}
                </div>
                <div class="flex-1 min-w-0">
                  <p class="font-bold text-white truncate">{{ caso.usuario?.nombre || 'Desconocido' }}</p>
                  <p class="text-[10px] text-despacho-fondo/40 truncate uppercase">{{ caso.usuario?.correo }}</p>
                </div>
              </div>
            </div>
  
            <div class="bg-white/5 p-6 rounded-3xl border border-white/10 shadow-xl space-y-6">
              <h3 class="text-[10px] font-bold uppercase tracking-widest text-despacho-tierra">Ficha Técnica</h3>
              
              <div>
                <span class="block text-[10px] font-bold uppercase text-despacho-fondo/30 mb-1">ID Expediente</span>
                <span class="font-mono text-despacho-fondo text-sm">#{{ caso.id }}</span>
              </div>
              
              <div>
                <span class="block text-[10px] font-bold uppercase text-despacho-fondo/30 mb-2">Rama Jurídica</span>
                <span v-if="!editando" class="font-medium text-white">{{ caso.ramaJuridica?.descripcion || 'General' }}</span>
                <select 
                  v-else
                  v-model.number="formularioEdicion.id_RamaJuridica"
                  class="w-full bg-black/30 border border-white/10 text-white rounded-lg px-3 py-2 text-xs focus:outline-none focus:ring-1 focus:ring-despacho-rojo"
                >
                  <option v-for="rama in listaRamas" :key="rama.id" :value="rama.id">{{ rama.descripcion }}</option>
                </select>
              </div>
            </div>

            <div v-if="!esCliente && !editando" class="mt-10 pt-6 border-t border-red-900/20">
              <button 
                @click="eliminarCaso"
                class="w-full py-3 flex items-center justify-center gap-2 text-red-500 hover:text-white hover:bg-red-600 border border-red-900/30 rounded-xl transition-all font-bold text-[10px] uppercase tracking-widest active:scale-95"
              >
                Eliminar Expediente
              </button>
            </div>
          </aside>
  
        </main>

        <div v-else class="flex flex-col items-center justify-center py-32 text-center border border-dashed border-white/10 rounded-3xl bg-black/10 mt-8">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-16 h-16 text-despacho-fondo/20 mb-4">
            <path stroke-linecap="round" stroke-linejoin="round" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
          </svg>
          <h2 class="text-2xl font-bold text-despacho-fondo/50">Expediente no disponible</h2>
          <p class="text-despacho-fondo/30 mt-2 max-w-md">El caso que buscas no existe o no tienes los permisos necesarios para visualizarlo.</p>
          <button @click="volver" class="mt-8 px-6 py-3 bg-despacho-rojo text-white font-bold rounded-xl hover:bg-despacho-rojo/90 transition-all active:scale-95 text-xs uppercase tracking-widest shadow-lg shadow-black/40">
            Regresar a la lista
          </button>
        </div>
  
      </div>
    </div>
</template>
  
<script setup>
import { ref, reactive, onMounted, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import axios from 'axios'
import Swal from 'sweetalert2'

const route = useRoute()
const router = useRouter()
const caso = ref(null)
const cargando = ref(true)
const editando = ref(false) 
const guardando = ref(false) 
const listaRamas = ref([]) 
const listaEstatus = ref([]) 
const historial = ref([]) 

const userRole = ref('')

const formularioEdicion = reactive({
  titulo: '',
  descripcion: '',
  id_RamaJuridica: null,
  id_estatus: null,
  id: null,
  id_usuario: null 
})

const decodificarJWT = (token) => {
  try {
    const base64Url = token.split('.')[1]
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/')
    return JSON.parse(window.atob(base64))
  } catch { return null }
}

const esCliente = computed(() => userRole.value === 'Cliente')

const volver = () => {
  if (esCliente.value) router.push('/mis-casos')
  else router.push('/casos')
}

const cargarDetalleCaso = async () => {
  const token = localStorage.getItem('token')
  if (!token) { router.push('/login'); return }
  try {
    const respuesta = await axios.get(`http://localhost:5039/api/casos/${route.params.id}`, {
      headers: { Authorization: `Bearer ${token}` }
    })
    caso.value = respuesta.data
  } catch (error) {
    console.error("Error al cargar el caso", error)
    
    if (error.response?.status === 403 || error.response?.status === 404) {
      await Swal.fire({
        icon: 'error',
        title: 'Acceso Denegado',
        text: 'No tienes permiso para ver este expediente o no existe.',
        background: '#1a1a1c',
        color: '#fdfbd4',
        confirmButtonColor: '#8B0000'
      })
      volver() 
    } else {
      Swal.fire({
        icon: 'error',
        title: 'Error de servidor',
        text: 'Ocurrió un problema de conexión.',
        background: '#1a1a1c',
        color: '#fdfbd4',
        confirmButtonColor: '#8B0000'
      })
    }
  } finally { 
    cargando.value = false 
  }
}

const cargarHistorial = async () => {
  const token = localStorage.getItem('token')
  try {
    const respuesta = await axios.get(`http://localhost:5039/api/casos/${route.params.id}/historial`, {
      headers: { Authorization: `Bearer ${token}` }
    })
    historial.value = respuesta.data
  } catch (error) { console.error(error) }
}

const cargarCatalogos = async () => {
  if (esCliente.value) return 
  const token = localStorage.getItem('token')
  try {
    const [resRamas, resEstatus] = await Promise.all([
      axios.get('http://localhost:5039/api/RamaJuridica', { headers: { Authorization: `Bearer ${token}` } }),
      axios.get('http://localhost:5039/api/Estatus', { headers: { Authorization: `Bearer ${token}` } })
    ])
    listaRamas.value = resRamas.data
    listaEstatus.value = resEstatus.data
  } catch (error) { console.error(error) }
}

const activarEdicion = () => {
  formularioEdicion.id = caso.value.id
  formularioEdicion.titulo = caso.value.titulo
  formularioEdicion.descripcion = caso.value.descripcion
  formularioEdicion.id_RamaJuridica = caso.value.id_RamaJuridica
  formularioEdicion.id_estatus = caso.value.id_estatus
  formularioEdicion.id_usuario = caso.value.id_usuario 
  editando.value = true 
}

const cancelarEdicion = () => { editando.value = false }

const guardarCambios = async () => {
  const token = localStorage.getItem('token')
  guardando.value = true
  try {
    await axios.put(`http://localhost:5039/api/casos/${formularioEdicion.id}`, formularioEdicion, {
      headers: { Authorization: `Bearer ${token}` }
    })
    await cargarDetalleCaso()
    await cargarHistorial() 
    editando.value = false
    Swal.fire({ toast: true, position: 'top-end', icon: 'success', title: 'Actualizado', showConfirmButton: false, timer: 2000, background: '#1a1a1c', color: '#fdfbd4' })
  } catch {
    Swal.fire({ icon: 'error', title: 'Error', text: 'No se pudo actualizar.', background: '#1a1a1c', color: '#fdfbd4' })
  } finally { guardando.value = false }
}

const eliminarCaso = async () => {
  const res = await Swal.fire({ title: '¿ELIMINAR?', text: "Esto es permanente.", icon: 'warning', showCancelButton: true, confirmButtonColor: '#8B0000', background: '#1a1a1c', color: '#fdfbd4' })
  if (!res.isConfirmed) return
  const token = localStorage.getItem('token')
  try {
    await axios.delete(`http://localhost:5039/api/casos/${route.params.id}`, { headers: { Authorization: `Bearer ${token}` } })
    router.push('/casos')
  } catch { Swal.fire('Error', 'No se pudo eliminar.', 'error') }
}

const formatearFechaHistorial = (fechaIso) => {
  const fecha = new Date(fechaIso)
  return fecha.toLocaleDateString('es-MX', { day: '2-digit', month: 'short' }).toUpperCase()
}

onMounted(() => {
  const token = localStorage.getItem('token')
  if (token) {
    const payload = decodificarJWT(token)
    userRole.value = payload ? payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] : ''
  }
  cargarDetalleCaso()
  cargarHistorial() 
  cargarCatalogos()
})
</script>