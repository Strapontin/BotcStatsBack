import { Fragment, useCallback, useEffect, useState } from "react";
import Title from "@/components/ui/title";
import {
  updateRole,
  getRoleById,
  getAllRoles,
} from "../../../../data/back-api/back-api";
import { Loading, Text } from "@nextui-org/react";
import classes from "../index.module.css";
import { Check, XOctagon } from "react-feather";
import RoleCreateEdit from "@/components/create-edit/role-create-edit/RoleCreateEdit";
import { Role, getNewEmptyRole } from "@/entities/Role";
import { useRouter } from "next/router";
import { toLowerRemoveDiacritics } from "@/helper/string";
import { CharacterType } from "@/entities/enums/characterType";
import { Alignment } from "@/entities/enums/alignment";

export default function UpdateRolePage() {
  const roleId: number = Number(useRouter().query.roleId);

  const [oldRole, setOldRole] = useState<Role>(getNewEmptyRole());

  const [roleCreateEditKey, setRoleCreateEditKey] = useState(0);
  const [message, setMessage] = useState(<Fragment />);
  const [role, setRole] = useState<Role>(getNewEmptyRole());

  const [roles, setRoles] = useState<string[]>([]);

  const canUpdateRole = useCallback(() => {
    if (role.name === "") {
      updateMessage(true, "Un nom est obligatoire.");
      return false;
    } else if (
      roles.filter(
        (p) =>
          toLowerRemoveDiacritics(p) !==
            toLowerRemoveDiacritics(oldRole.name) &&
          toLowerRemoveDiacritics(p) === toLowerRemoveDiacritics(role.name)
      ).length !== 0
    ) {
      updateMessage(true, "Un rôle avec ce nom existe déjà.");
      return false;
    } else {
      updateMessage(false, "");
      return true;
    }
  }, [role, roles, oldRole]);

  useEffect(() => {
    if (roleId === undefined || isNaN(roleId)) return;

    async function initRole() {
      const e = await getRoleById(roleId);
      setRole(e);
      setOldRole(e);
    }
    initRole();

    async function initRoles() {
      const e = (await getAllRoles()).map((e) => e.name);
      setRoles(e);
    }
    initRoles();
  }, [roleId]);

  // Updates message on component refreshes
  useEffect(() => {
    if (
      role.name === oldRole.name &&
      role.characterType === oldRole.characterType &&
      role.alignment === oldRole.alignment
    )
      return;

    if (toLowerRemoveDiacritics(role.name) === "") {
      updateMessage(true, "Un nom est obligatoire.");
    } else if (
      roles.filter(
        (p) => toLowerRemoveDiacritics(p) === toLowerRemoveDiacritics(role.name)
      ).length !== 0
    ) {
      updateMessage(true, "Un module avec ce nom existe déjà.");
    } else {
      setMessage(<Fragment />);
    }
  }, [role, roles, oldRole]);

  if (role.id === -1) {
    return (
      <Fragment>
        <Loading />
      </Fragment>
    );
  }

  const title = <Title>Modification du module {`'${oldRole.name}'`}</Title>;

  async function btnUpdateRole() {
    if (!canUpdateRole()) return;

    if (await updateRole(role)) {
      const r = await getRoleById(roleId);
      setRole(r);
      setRoleCreateEditKey(roleCreateEditKey + 1);
      setTimeout(
        () =>
          updateMessage(
            false,
            `Le rôle '${role.name}' a été modifié correctement.`
          ),
        50
      );
    } else {
      //Erreur
      updateMessage(
        true,
        "Une erreur est survenue lors de la modification du rôle."
      );
    }
  }

  function updateMessage(isError: boolean, message: string) {
    if (message === "") {
      setMessage(<Fragment />);
    } else if (isError) {
      setMessage(
        <Text span className={classes.red}>
          <XOctagon className={classes.icon} />
          {message}
        </Text>
      );
    } else {
      setMessage(
        <Text span className={classes.green}>
          <Check className={classes.icon} />
          {message}
        </Text>
      );
    }
  }

  return (
    <RoleCreateEdit
      key={roleCreateEditKey}
      title={title}
      role={role}
      setRole={setRole}
      message={message}
      btnPressed={btnUpdateRole}
      btnText="Modifier le rôle"
    />
  );
}
