import { Role, getNewEmptyRole } from "@/entities/Role";
import { Input, Loading, Spacer } from "@nextui-org/react";
import { Fragment, useState } from "react";
import { toLowerRemoveDiacritics } from "../../helper/string";
import Classes from "./AutocompleteAddRole.module.css";
import { X } from "react-feather";
import ListItemRole from "../list-stats/ListItemRole";

export default function AutocompleteAddRole(props: {
  roles: Role[];
  onDelete: any;
  onSelectRole: any;
  roleSelected?: Role;
}) {
  const allRoles = props.roles;
  const [visibleRoles, setVisibleRoles] = useState<Role[]>(
    getAllVisibleRoles("")
  );
  const [roleSelected, setRoleSelected] = useState<Role>(getInitRoleSelected());
  const [isVisible, setIsVisible] = useState(false);
  if (props.roles.length === 0) return <Loading />;

  function getInitRoleSelected() {
    if (props.roleSelected !== undefined) return props.roleSelected;
    return getNewEmptyRole();
  }

  // console.log("roleSelected ==");
  // console.log(roleSelected);

  // All roles filtered with the input
  function getAllVisibleRoles(filter: string) {
    return allRoles.filter((role) =>
      toLowerRemoveDiacritics(role.name).includes(
        toLowerRemoveDiacritics(filter)
      )
    );
  }

  // When the user types
  function inputChanged(inputValue: string) {
    setVisibleRoles(getAllVisibleRoles(inputValue));
  }

  // Hides/Show the list of roles filtered
  function setValuesToSelectVisible(event: any, visible: boolean) {
    // console.log(event);
    const timeout = visible ? 0 : 500;
    setTimeout(() => {
      setIsVisible(visible);
      setVisibleRoles(getAllVisibleRoles(""));
    }, timeout);
  }

  function onSelectRole(id: number) {
    const roleSelected = allRoles.filter((role) => role.id === id)[0];
    setRoleSelected(roleSelected);
    props.onSelectRole(roleSelected);
  }

  return (
    <Fragment>
      {roleSelected.id === -1 && (
        <div className={Classes["autocomplete-role"]}>
          <div className={Classes["input-container"]}>
            <Input
              css={{ flex: 1 }}
              labelPlaceholder="Rôle"
              aria-label="Rôle"
              clearable
              bordered
              // value={roleSelected}
              onChange={(event) => inputChanged(event.target.value)}
              onFocus={(event) => setValuesToSelectVisible(event, true)}
              onClearClick={() => inputChanged("")}
              onBlur={(event) => setValuesToSelectVisible(event, false)}
            ></Input>
            <Spacer x={0.75} />
            <X className={Classes.delete} onClick={props.onDelete} />
          </div>
          {isVisible && <Spacer y={0.75} />}
          {isVisible && (
            <div className={Classes["container-roles-values"]}>
              {visibleRoles.map((role) => (
                <div key={role.id} className={Classes["role-item"]}>
                  <ListItemRole
                    image={role.name}
                    characterType={role.characterType}
                    onClick={() => onSelectRole(role.id)}
                  />
                  <Spacer y={0.75} />
                </div>
              ))}
            </div>
          )}
        </div>
      )}
      {roleSelected.id !== -1 && (
        <div className={Classes["selected-role-container"]}>
          <ListItemRole
            image={roleSelected.name}
            characterType={roleSelected.characterType}
          ></ListItemRole>
          <Spacer x={0.75} />
          <X className={Classes.delete} onClick={props.onDelete} />
        </div>
      )}
    </Fragment>
  );
}
