import React from 'react';
import {
  StyleSheet,
  Text,
  View,
  TouchableOpacity,
  ImageBackground,
  Image,
  StatusBar,
} from 'react-native';
import { useRouter } from 'expo-router';
import { CheckCircle2, Landmark, LayoutList } from 'lucide-react-native';

export default function RegisterOrgSuccessScreen() {
  const router = useRouter();

  const handleGoToDashboard = () => {
    router.replace('/(main)/institution-home');
  };

  return (
    <>
      <StatusBar
        translucent
        backgroundColor="transparent"
        barStyle="light-content"
      />

      {/* Fondo de pantalla de los naranjos */}
      <ImageBackground
        // Reemplaza esto con la ruta real de tu foto de fondo
        source={require('../../../assets/images/logo.png')}
        style={styles.background}
      >
        {/* Capa oscura semi-transparente para que resalte la tarjeta */}
        <View style={styles.overlay}>
          <View style={styles.contentContainer}>
            {/* Logo de CitraNica (Fuera de la tarjeta) */}
            <View style={styles.logoHeader}>
              <Image
                source={require('../../../assets/images/logo.png')}
                style={styles.logoImage}
                resizeMode="contain"
              />
              <Text style={styles.logoText}>
                Citra<Text style={styles.logoTextOrange}>Nica</Text>
              </Text>
            </View>

            {/* Tarjeta Blanca Central */}
            <View style={styles.card}>
              {/* Composición de Íconos (Check y Edificio) */}
              <View style={styles.iconContainer}>
                {/* Edificio naranja (Fondo) */}
                <View style={styles.buildingIcon}>
                  <Landmark color="#ED8936" size={50} strokeWidth={2} />
                </View>
                {/* Check verde gigante (Frente) */}
                <View style={styles.checkIcon}>
                  <CheckCircle2
                    color="#45C264"
                    size={70}
                    strokeWidth={2.5}
                    fill="#FFFFFF"
                  />
                </View>
              </View>

              {/* Textos de la tarjeta */}
              <Text style={styles.title}>
                ¡Cuenta creada{'\n'}Exitosamente!
              </Text>

              <Text style={styles.greeting}>Estimado Usuario:</Text>
              <Text style={styles.bodyText}>
                Tu organización ya forma parte de CitraNica. Desde tu panel
                podrás publicar comunicados, eventos, documentos y formularios
                para mantener informada a la comunidad.
              </Text>

              {/* Botón "Ir al panel" */}
              <TouchableOpacity
                style={styles.actionButton}
                onPress={handleGoToDashboard}
              >
                <LayoutList
                  color="#45C264"
                  size={20}
                  style={styles.buttonIcon}
                />
                <Text style={styles.buttonText}>Ir al panel</Text>
              </TouchableOpacity>
            </View>
          </View>
        </View>
      </ImageBackground>
    </>
  );
}

// --- ESTILOS ---

const styles = StyleSheet.create({
  background: {
    flex: 1,
    width: '100%',
    height: '100%',
  },
  overlay: {
    flex: 1,
    backgroundColor: 'rgba(0, 0, 0, 0.55)', // El filtro oscuro sobre la foto
    justifyContent: 'center',
    alignItems: 'center',
    paddingHorizontal: 24,
  },
  contentContainer: {
    width: '100%',
    alignItems: 'center',
  },

  // Header Logo
  logoHeader: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 20,
  },
  logoImage: {
    width: 35,
    height: 35,
    marginRight: 8,
  },
  logoText: {
    fontSize: 28,
    fontWeight: '800',
    color: '#FFFFFF',
    letterSpacing: -0.5,
  },
  logoTextOrange: {
    color: '#ED8936',
  },

  // Card Blanca
  card: {
    backgroundColor: '#FFFFFF',
    width: '100%',
    borderRadius: 24,
    padding: 30,
    alignItems: 'center',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 10 },
    shadowOpacity: 0.25,
    shadowRadius: 20,
    elevation: 10,
  },

  // Íconos solapados
  iconContainer: {
    width: 100,
    height: 100,
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: 16,
  },
  buildingIcon: {
    position: 'absolute',
    right: 5,
    top: 5,
  },
  checkIcon: {
    position: 'absolute',
    left: 0,
    bottom: 5,
    backgroundColor: '#FFFFFF', // Para tapar la línea del edificio detrás
    borderRadius: 40,
  },

  // Textos
  title: {
    fontSize: 24,
    fontWeight: '800',
    color: '#2F855A', // Verde oscuro
    textAlign: 'center',
    marginBottom: 20,
    lineHeight: 28,
  },
  greeting: {
    fontSize: 14,
    fontWeight: '700',
    color: '#1A2E2B',
    alignSelf: 'flex-start', // Alineado a la izquierda
    marginBottom: 8,
  },
  bodyText: {
    fontSize: 13,
    color: '#4A5568',
    textAlign: 'left',
    lineHeight: 20,
    marginBottom: 30,
  },

  // Botón Verde Claro
  actionButton: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: '#F0FDF4', // Verde super clarito
    borderWidth: 1.5,
    borderColor: '#45C264', // Borde verde
    borderRadius: 25,
    width: '100%',
    height: 50,
  },
  buttonIcon: {
    marginRight: 10,
  },
  buttonText: {
    fontSize: 16,
    fontWeight: '700',
    color: '#2F855A', // Verde oscuro para contraste
  },
});
