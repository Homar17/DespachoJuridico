<template>
  <div class="fixed inset-0 bg-black/60 backdrop-blur-sm flex items-center justify-center p-4 z-50 transition-opacity duration-300">
    <div class="bg-despacho-oscuro text-despacho-fondo w-full max-w-lg rounded-3xl shadow-2xl overflow-hidden border border-despacho-fondo/10">
      
      <div class="p-6 pb-0 flex justify-between items-start">
        <div>
          <h3 class="text-2xl font-bold tracking-tight">Nuevo Expediente</h3>
          <div class="h-1 w-16 bg-despacho-rojo rounded-full mt-1.5"></div>
        </div>
        <button @click="$emit('cerrar')" class="text-despacho-fondo/50 hover:text-white transition-colors">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2.5" stroke="currentColor" class="w-6 h-6">
            <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>

      <form @submit.prevent="enviarFormulario" class="p-8 space-y-5">
        
        <div>
          <label class="block text-xs font-bold uppercase tracking-wider text-despacho-tierra mb-1.5">Título del Caso</label>
          <input 
            v-model="datos.titulo" 
            type="text" 
            placeholder="Ej. Divorcio Incausado - Pérez"
            class="w-full bg-white/5 border border-despacho-fondo/20 text-despacho-fondo rounded-xl px-4 py-3 focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all placeholder:text-despacho-fondo/20"
            required 
          />
        </div>

        <div>
          <label class="block text-xs font-bold uppercase tracking-wider text-despacho-tierra mb-1.5">Descripción</label>
          <textarea 
            v-model="datos.descripcion" 
            rows="3" 
            placeholder="Detalles preliminares del caso..."
            class="w-full bg-white/5 border border-despacho-fondo/20 text-despacho-fondo rounded-xl px-4 py-3 focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all placeholder:text-despacho-fondo/20"
          ></textarea>
        </div>

        <div class="grid grid-cols-2 gap-5">
          <div>
            <label class="block text-xs font-bold uppercase tracking-wider text-despacho-tierra mb-1.5">ID Rama</label>
            <input 
              v-model.number="datos.id_RamaJuridica" 
              type="number" 
              class="w-full bg-white/5 border border-despacho-fondo/20 text-despacho-fondo rounded-xl px-4 py-3 focus:outline-none focus:ring-2 focus:ring-despacho-rojo"
              required 
            />
          </div>
          <div>
            <label class="block text-xs font-bold uppercase tracking-wider text-despacho-tierra mb-1.5">ID Estatus</label>
            <input 
              v-model.number="datos.id_estatus" 
              type="number" 
              class="w-full bg-white/5 border border-despacho-fondo/20 text-despacho-fondo rounded-xl px-4 py-3 focus:outline-none focus:ring-2 focus:ring-despacho-rojo"
              required 
            />
          </div>
        </div>

        <div class="pt-6 flex gap-4">
          <button 
            type="button" 
            @click="$emit('cerrar')" 
            class="flex-1 py-3 border border-despacho-fondo/30 text-despacho-fondo/70 font-semibold rounded-xl hover:bg-white/5 hover:text-white transition-all active:scale-95"
          >
            Cancelar
          </button>
          <button 
            type="submit" 
            class="flex-1 py-3 bg-despacho-rojo text-white font-bold rounded-xl shadow-lg hover:bg-despacho-rojo/90 transition-all active:scale-95 shadow-despacho-rojo/30"
          >
            Guardar Expediente
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { reactive } from 'vue'

const emit = defineEmits(['cerrar', 'creado'])

const datos = reactive({
  titulo: '',
  descripcion: '',
  id_usuario: 1, // ID del Admin por defecto
  id_estatus: 1, // ID del estatus 'Abierto'
  id_RamaJuridica: 1 // ID de la rama 'Familiar'
})

const enviarFormulario = () => {
  emit('creado', { ...datos })
  // Limpiar formulario después de enviar
  datos.titulo = ''
  datos.descripcion = ''
}
</script>