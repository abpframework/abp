import { connect } from 'react-redux';

export function connectToRedux({ component, stateProps = () => ({}), dispatchProps = () => ({}) }) {
  const mapStateToProps = () => stateProps;

  const mapDispatchToProps = dispatchProps;

  return connect(mapStateToProps, mapDispatchToProps)(component);
}
