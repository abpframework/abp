import i18n from 'i18n-js';
import { Text } from 'native-base';
import React from 'react';
import { StyleSheet, View } from 'react-native';

function HomeScreen() {
  return (
    <View style={styles.container}>
      <Text style={styles.title}> {i18n.t('::Welcome')}</Text>
      <Text style={styles.centeredText}> {i18n.t('::LongWelcomeMessage')}</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    paddingHorizontal: 20,
    backgroundColor: '#fff',
  },
  centeredText: {
    textAlign: 'center',
  },
  title: {
    marginBottom: 30,
    fontSize: 32,
    fontWeight: '300',
    textAlign: 'center',
  },
});

export default HomeScreen;
